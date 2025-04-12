import 'package:gowheel_flutterflow_ui/service/storage_service.dart';
import 'package:http/http.dart' as http;
import 'package:url_launcher/url_launcher.dart';

import '../components/snackbar.dart';
import '../url.dart';

class MomoPaymentService {
  final TokenService tokenService = TokenService();

  Future<void> createAndLaunchMomoPayment(int bookingId) async {
    var token = await tokenService.getToken();
    try {
      var request = http.MultipartRequest(
        'POST',
        Uri.parse('${URL.baseUrl}User/Invoice/CreateInvoice/$bookingId'),
      );
      
      request.headers.addAll({
        'Authorization': 'Bearer $token',
        'accept': 'text/plain',
      });
      
      var response = await request.send();
      
      if (response.statusCode == 200) {
        Snackbar.showSuccess('Payment', 'Payment created successfully');
      } else {
        throw Exception('Failed to create payment: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Error creating payment: $e');
    }
  }

  // Future<void> createAndLaunchMomoPayment(int invoiceId) async {
  //   var token = await tokenService.getToken();
  //   try {
  //     var request = http.MultipartRequest(
  //       'POST',
  //       Uri.parse('${URL.baseUrl}User/Invoice/MomoPayment/$invoiceId'),
  //     );
      
  //     request.headers.addAll({
  //       'Authorization': 'Bearer $token',
  //       'accept': 'text/plain',
  //     });
      
  //     var response = await request.send();
  //     var paymentUrl = await response.stream.bytesToString();
  //     final uri = Uri.parse(paymentUrl);
  //     print('Payment URL from API: $paymentUrl'); // Log URL

  //     if (response.statusCode == 200) {
  //       final success = await launchUrl(uri);
  //       if (!success) {
  //         throw Exception('Could not launch MoMo app');
  //       }
  //     } else {
  //       throw Exception('Failed to create payment: ${response.statusCode}');
  //     }
  //   } catch (e) {
  //     throw Exception('Error creating payment: $e');
  //   }
  // }
}