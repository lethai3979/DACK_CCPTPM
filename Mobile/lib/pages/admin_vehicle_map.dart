import 'dart:async';
import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:gowheel_flutterflow_ui/components/snackbar.dart';
import 'package:gowheel_flutterflow_ui/service/storage_service.dart';
import 'package:gowheel_flutterflow_ui/url.dart';
import 'package:signalr_netcore/signalr_client.dart';

class AdminVehicleMapPage extends StatefulWidget {
  final String vehicleId;

  const AdminVehicleMapPage({super.key, required this.vehicleId});

  @override
  State<AdminVehicleMapPage> createState() => _AdminVehicleMapPageState();
}

class _AdminVehicleMapPageState extends State<AdminVehicleMapPage> {
  GoogleMapController? _mapController;
  HubConnection? _trackingHubConnection;
  final TokenService _tokenService = TokenService();

  LatLng? _currentPosition;
  Marker? _vehicleMarker;
  bool _isConnecting = true;
  bool _isConnected = false;
  String _connectionStatus = "Đang kết nối tới Hub...";
  StreamSubscription? _hubSubscription;

   BitmapDescriptor? _vehicleIcon;

  final LatLng _initialPosition = const LatLng(10.7769, 106.7009);

  @override
  void initState() {
    super.initState();
    _connectToTrackingHub();
  }

  @override
  void dispose() {
    _leaveAndDisconnect();
    _mapController?.dispose();
    super.dispose();
  }
  
  Future<void> _connectToTrackingHub() async {
    setState(() {
      _isConnecting = true;
      _connectionStatus = "Đang lấy token...";
    });

    final token = await _tokenService.getToken();
    if (token == null) {
      setState(() {
        _isConnecting = false;
        _connectionStatus = "Lỗi: Không tìm thấy token.";
      });
      Snackbar.showError("Lỗi", "Không thể xác thực. Vui lòng đăng nhập lại.");
      Get.back();
      return;
    }

    final hubUrl = '${URL.hubUrl}tracking-hub';
    print('Kết nối tới Tracking Hub: $hubUrl với Group: ${widget.vehicleId}');

    _trackingHubConnection = HubConnectionBuilder()
        .withUrl(
          hubUrl,
          options: HttpConnectionOptions(
            accessTokenFactory: () async => token,
          ),
        )
        .withAutomaticReconnect()
        .build();

    _trackingHubConnection?.on("ReceiveMessage", _handleReceiveLocationMessage);

    setState(() {
      _connectionStatus = "Đang kết nối tới hub...";
    });

    try {
      await _trackingHubConnection?.start();
      print('Đã kết nối tới Tracking Hub.');
       if (mounted) {
        setState(() {
          _isConnecting = false;
          _isConnected = true;
          _connectionStatus = "Đã kết nối. Đang tham gia nhóm...";
        });
       }
      await _joinGroup();

    } catch (e) {
      print('Lỗi kết nối tới Tracking Hub: $e');
      if (mounted) {
        setState(() {
          _isConnecting = false;
          _isConnected = false;
          _connectionStatus = "Lỗi kết nối: $e";
        });
      }
      Snackbar.showError("Lỗi kết nối Hub", "Không thể kết nối tới tracking hub: $e");
    }
  }

  Future<void> _joinGroup() async {
    if (_trackingHubConnection?.state == HubConnectionState.Connected) {
      try {
        await _trackingHubConnection?.invoke("AddToGroup", args: [widget.vehicleId]);
        print('Đã tham gia nhóm: ${widget.vehicleId}');
        if (mounted) {
          setState(() {
            _connectionStatus = "Đang theo dõi xe ${widget.vehicleId}";
          });
        }
      } catch (e) {
        print('Lỗi khi tham gia nhóm ${widget.vehicleId}: $e');
         if (mounted) {
          setState(() {
            _connectionStatus = "Lỗi khi tham gia nhóm: $e";
          });
         }
        Snackbar.showError("Lỗi Nhóm", "Không thể tham gia nhóm theo dõi: $e");
      }
    } else {
      print('Không thể tham gia nhóm vì chưa kết nối.');
       if (mounted) {
          setState(() {
            _connectionStatus = "Chưa kết nối để tham gia nhóm.";
          });
       }
    }
  }

  Future<void> _leaveAndDisconnect() async {
    print("Leaving group and disconnecting...");
    if (_trackingHubConnection != null) {
      if (_trackingHubConnection?.state == HubConnectionState.Connected) {
        try {
          print('Đang rời khỏi nhóm: ${widget.vehicleId}');
          await _trackingHubConnection?.invoke("RemoveFromGroup", args: [widget.vehicleId]);
          print('Đã rời khỏi nhóm: ${widget.vehicleId}');
        } catch (e) {
          print('Lỗi khi rời nhóm ${widget.vehicleId}: $e');
        }
      }
      try {
        await _trackingHubConnection?.stop();
        print('Đã ngắt kết nối khỏi Tracking Hub.');
         if (mounted) {
            setState(() {
              _isConnected = false;
              _connectionStatus = "Đã ngắt kết nối.";
            });
         }
      } catch (e) {
        print('Lỗi khi ngắt kết nối Tracking Hub: $e');
      }
      _trackingHubConnection = null;
    }
     _hubSubscription?.cancel();
     _hubSubscription = null;
  }

  void _handleReceiveLocationMessage(List<Object?>? arguments) {
    if (!mounted) return;

    print('Tin nhắn vị trí nhận được: $arguments');
    if (arguments != null && arguments.isNotEmpty) {
      final messageData = arguments[0];

      if (messageData is String) {
        try {
          final locationData = jsonDecode(messageData);
          final latitude = locationData['lat'];
          final longitude = locationData['log'];

          if (latitude is num && longitude is num) {
            final newPosition = LatLng(latitude.toDouble(), longitude.toDouble());

            setState(() {
              _currentPosition = newPosition;
              _vehicleMarker = Marker(
                markerId: MarkerId(widget.vehicleId),
                position: newPosition,
                infoWindow: InfoWindow(
                  title: 'Xe ${widget.vehicleId}',
                  snippet: 'Vị trí: ${newPosition.latitude.toStringAsFixed(6)}, ${newPosition.longitude.toStringAsFixed(6)}',
                ),
                icon: BitmapDescriptor.defaultMarkerWithHue(BitmapDescriptor.hueAzure),
                rotation: _vehicleMarker?.rotation ?? 0,
              );
              _connectionStatus = "Đã cập nhật vị trí lúc ${DateTime.now().toIso8601String()}";
            });

            _mapController?.animateCamera(
              CameraUpdate.newLatLng(newPosition),
            );
          } else {
             print('Dữ liệu latitude/longitude không hợp lệ trong JSON: $messageData');
             Snackbar.showWarning("Dữ liệu không hợp lệ", "Định dạng lat/lon không đúng.");
          }
        } catch (e) {
          print('Lỗi giải mã JSON vị trí: $e');
          print('Chuỗi JSON nhận được: $messageData');
          Snackbar.showWarning("Lỗi dữ liệu", "Không thể đọc dữ liệu vị trí nhận được.");
           if (mounted) {
            setState(() {
               _connectionStatus = "Lỗi xử lý dữ liệu vị trí.";
            });
           }
        }
      } else {
         print('Dữ liệu tin nhắn không phải là chuỗi: $messageData');
         if (mounted) {
            setState(() {
               _connectionStatus = "Nhận được dữ liệu không mong đợi.";
            });
         }
      }
    }
  }

  void _onMapCreated(GoogleMapController controller) {
    _mapController = controller;
    if (_currentPosition != null) {
       _mapController?.animateCamera(CameraUpdate.newLatLngZoom(_currentPosition!, 15));
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: const Color(0xFF80EE98),
        title: Text(
          'Theo dõi xe ${widget.vehicleId}',
          style: GoogleFonts.lexendDeca(
            color: const Color(0xFF213A58),
            fontSize: 20.0,
            fontWeight: FontWeight.bold,
          ),
        ),
        leading: IconButton(
          icon: const Icon(Icons.arrow_back_rounded, color: Color(0xFF213A58)),
          onPressed: () => Get.back(),
        ),
      ),
      body: Stack(
        children: [
          GoogleMap(
            onMapCreated: _onMapCreated,
            initialCameraPosition: CameraPosition(
              target: _currentPosition ?? _initialPosition,
              zoom: 14.0,
            ),
            markers: _vehicleMarker != null ? {_vehicleMarker!} : {},
            mapType: MapType.normal,
            myLocationEnabled: true,
            myLocationButtonEnabled: true,
             zoomControlsEnabled: true,
          ),
          Positioned(
            top: 0,
            left: 0,
            right: 0,
            child: Container(
              padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
              color: _isConnected ? Colors.green.withOpacity(0.8) : Colors.orange.withOpacity(0.8),
              child: Text(
                _connectionStatus,
                style: const TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
                textAlign: TextAlign.center,
              ),
            ),
          ),
          if (_isConnecting)
            Container(
              color: Colors.black.withOpacity(0.3),
              child: const Center(
                child: CircularProgressIndicator(color: Colors.white,),
              ),
            ),
        ],
      ),
    );
  }
}