// import 'package:flutter/material.dart';
// import 'package:get/get.dart';
// import 'package:gowheel_flutterflow_ui/pages/sign_in.dart';
// import 'package:gowheel_flutterflow_ui/root.dart';
// import 'package:gowheel_flutterflow_ui/service/storage_service.dart';

// void main() {
//   runApp(const MyApp());
// }

// class MyApp extends StatelessWidget {
//   const MyApp({super.key});

//   @override
//   Widget build(BuildContext context) {
//     TokenService tokenService = TokenService();
  
//     return GetMaterialApp(
//       debugShowCheckedModeBanner: false,
//       theme: ThemeData(
//         primaryColor:const Color(0xFF213A58) ,
//         colorScheme: const ColorScheme.light(
//           primary: Color(0xFF213A58),
//           onPrimary: Colors.white
//         ),
//       ),
//       home: tokenService.getToken().isNull ? const SignInWidget(): const RootPage(),
//     );

//   }
// }
import 'package:flutter/material.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'dart:math';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: MapScreen(),
    );
  }
}

class MapScreen extends StatefulWidget {
  @override
  _MapScreenState createState() => _MapScreenState();
}

class _MapScreenState extends State<MapScreen> {
  late GoogleMapController mapController;
  LatLng currentPosition = LatLng(21.0278, 105.8342); // Hà Nội
  Set<Marker> markers = {};

  @override
  void initState() {
    super.initState();
    _startSimulation();
    // Thêm marker ban đầu
    markers.add(
      Marker(
        markerId: MarkerId('xe'),
        position: currentPosition,
        infoWindow: InfoWindow(title: 'Vị trí xe'),
      ),
    );
  }

  void _startSimulation() {
    // Cập nhật vị trí ngẫu nhiên mỗi 2 giây
    Future.doWhile(() async {
      await Future.delayed(Duration(seconds: 2));
      if (mounted) {
        setState(() {
          // Tạo vị trí ngẫu nhiên quanh Hà Nội
          currentPosition = LatLng(
            currentPosition.latitude + (Random().nextDouble() * 0.001 - 0.0005),
            currentPosition.longitude + (Random().nextDouble() * 0.001 - 0.0005),
          );
          // Cập nhật marker
          markers.removeWhere((marker) => marker.markerId.value == 'xe');
          markers.add(
            Marker(
              markerId: MarkerId('xe'),
              position: currentPosition,
              infoWindow: InfoWindow(title: 'Vị trí xe'),
            ),
          );
        });
        mapController.animateCamera(CameraUpdate.newLatLng(currentPosition));
      }
      return true; // Tiếp tục vòng lặp
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: GoogleMap(
        onMapCreated: (controller) {
          mapController = controller;
          mapController.animateCamera(CameraUpdate.newLatLng(currentPosition));
        },
        initialCameraPosition: CameraPosition(
          target: currentPosition,
          zoom: 15,
        ),
        markers: markers,
      ),
    );
  }

  @override
  void dispose() {
    super.dispose();
  }
}