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
  final String _hubUrl = '${URL.hubUrl}notifyhub';
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

      // Register listener for "ReceiveMessage"
      _hubConnection?.on("ReceiveMessage", (arguments) {
        _handleReceiveMessage(arguments);
      });

      await _hubConnection?.start();
      print('Connected to SignalR Hub');
    } catch (e) {
      print('Error connecting to SignalR Hub: $e');
    }
  }

  void _handleReceiveMessage(List<Object?>? arguments) {
    if (arguments != null && arguments.isNotEmpty) {
      print('Message received: ${arguments[0]}');
      Snackbar.showSuccess("Message", arguments[0] as String);
      // Refresh notifications when new message is received
      if(arguments[0] == 'Your booking confirmed' || arguments[0] ==  'Your booking has been denied') 
        _bookingController.fetchBookings();
      _controller.fetchNotifications();
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
}