
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:gowheel_flutterflow_ui/controllers/invoice_controller.dart';

import '../controllers/payment_controller.dart';
import '../service/momo_payment_service.dart';

class PaymentButton extends StatelessWidget {
  final int invoiceId;

  const PaymentButton({
    Key? key,
    required this.invoiceId,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    // Initialize with MomoPaymentService
    final invoicecontroller = Get.put(InvoiceController());
    final controller = Get.put(PaymentController(MomoPaymentService()));

    return Container(
      width: double.infinity,
      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
      child: Obx(() => ElevatedButton(
        onPressed: controller.isProcessing.value
            ? null
            : () => controller.processPayment(invoiceId),
        style: ElevatedButton.styleFrom(
          backgroundColor: const Color(0xFF80EE98),
          foregroundColor: const Color(0xFF213A58),
          padding: const EdgeInsets.symmetric(vertical: 16),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12),
          ),
          elevation: 2,
          disabledBackgroundColor: const Color(0xFF80EE98).withOpacity(0.6),
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            if (controller.isProcessing.value) ...[
              const SizedBox(
                height: 24,
                width: 24,
                child: CircularProgressIndicator(
                  color: Color(0xFF213A58),
                  strokeWidth: 2.5,
                ),
              ),
              const SizedBox(width: 12),
            ],
            Text(
              controller.isProcessing.value ? 'Processing Payment...' : 'Pay Now',
              style: GoogleFonts.lexendDeca(
                fontSize: 16,
                fontWeight: FontWeight.w600,
              ),
            ),
            if (!controller.isProcessing.value) ...[
              const SizedBox(width: 8),
              const Icon(Icons.payment, size: 20),
            ],
          ],
        ),
      )),
    );
  }
}