import 'package:signalr_netcore/signalr_client.dart';
import 'package:get/get.dart';
import '../components/snackbar.dart';
import '../controllers/booking_controller.dart';
import '../controllers/notification_controller.dart';
import 'package:gowheel_flutterflow_ui/service/storage_service.dart';
import '../url.dart';

class HubService {
  static HubService? _instance;
  HubConnection? _hubConnection;
  HubConnection? _trackingHubConnection;
  final String _hubUrl = '${URL.hubUrl}notifyhub';
  final String _trackingHubUrl = '${URL.hubUrl}tracking-hub';
  final _tokenService = TokenService();
  final NotificationController _controller = Get.put(NotificationController());
  final BookingController _bookingController = Get.put(BookingController()); 

  static HubService get instance {
    _instance ??= HubService._internal();
    return _instance!;
  }

  HubService._internal();

  Future<void> connect() async {
    try {
      final token = await _tokenService.getToken();

      _hubConnection = HubConnectionBuilder()
          .withUrl(
            _hubUrl,
            options: HttpConnectionOptions(
              accessTokenFactory: () async => token as String,
            ),
          )
          .build();

      _hubConnection?.on("ReceiveMessage", (arguments) {
        _handleReceiveMessage(arguments);
      });

      await _hubConnection?.start();
      print('Connected to SignalR Hub');
    } catch (e) {
      print('Error connecting to SignalR Hub: $e');
    }
  }

  Future<void> connecttrackinghub() async {
    try {
      final token = await _tokenService.getToken();

      _trackingHubConnection = HubConnectionBuilder()
          .withUrl(
            _trackingHubUrl,
            options: HttpConnectionOptions(
              accessTokenFactory: () async => token as String,
            ),
          )
          .build();

      _trackingHubConnection?.on("ReceiveMessage", (arguments) {
        _handleReceiveLocationMessage(arguments);
      });
      
      await _trackingHubConnection?.start();
      _addtogroup("group1");
      
      print('Connected to SignalR tracking Hub');
    } catch (e) {
      print('Error connecting to SignalR Hub: $e');
    }
  }

  void _addtogroup(String groupName) {
    _trackingHubConnection?.invoke("AddToGroup", args: [groupName]);
  }

  void _handleReceiveMessage(List<Object?>? arguments) {
    if (arguments != null && arguments.isNotEmpty) {
      print('Message received: ${arguments[0]}');
      Snackbar.showSuccess("Message", arguments[0] as String);
      if(arguments[0] == 'Your booking confirmed' || arguments[0] ==  'Your booking has been denied') 
        _bookingController.fetchBookings();
      _controller.fetchNotifications();
    }
  }

  void _handleReceiveLocationMessage(List<Object?>? arguments) {
    if (arguments != null && arguments.isNotEmpty) {
      print('Message received from tracking hub: ${arguments[0]}');
      Snackbar.showSuccess("Message", arguments[0] as String);
    }
  }

  Future<void> disconnect() async {
    try {
      await _hubConnection?.stop();
      print('Disconnected from SignalR Hub');
    } catch (e) {
      print('Error disconnecting from SignalR Hub: $e');
    }
  }

  Future<void> disconnectLocationHub() async {
    try {
      await _trackingHubConnection?.stop();
      print('Disconnected from SignalR tracking Hub');
    } catch (e) {
      print('Error disconnecting from SignalR Hub: $e');
    }
  }
}