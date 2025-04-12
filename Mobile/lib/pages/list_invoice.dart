import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:skeletonizer/skeletonizer.dart';
import 'package:google_fonts/google_fonts.dart';

import '../controllers/invoice_controller.dart';
import '../models/invoice_model.dart';

class InvoiceScreen extends StatelessWidget {
  const InvoiceScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: const Color(0xFF80EE98),
        title: Text(
          'Your Invoices',
          style: GoogleFonts.lexendDeca(
            color: const Color(0xFF213A58),
            fontSize: 20.0,
            fontWeight: FontWeight.bold,
          ),
        ),
      ),
      body: GetX<InvoiceController>(
        init: InvoiceController(),
        builder: (controller) {
          return Column(
            children: [
              Expanded(
                child: RefreshIndicator(
                  onRefresh: () async {
                    await controller.refreshInvoices();
                  },
                  child: controller.isLoading.value
                      ? const Center(child: CircularProgressIndicator())
                      : Skeletonizer(
                          enabled: controller.isLoading.value,
                          child: ListView.builder(
                            padding: const EdgeInsets.all(16),
                            itemCount: controller.filteredInvoices.length,
                            itemBuilder: (context, index) {
                              final invoice = controller.filteredInvoices[index];
                              return InvoiceCard(
                                invoice: invoice,
                                isLoading: controller.isLoading.value,
                              );
                            },
                          ),
                        ),
                ),
              ),
            ],
          );
        },
      ),
    );
  }

  Widget _buildFilterChip({
    required String label,
    required bool selected,
    required VoidCallback onSelected,
    required Color color,
  }) {
    return FilterChip(
      label: Text(
        label,
        style: TextStyle(
          color: selected ? Colors.white : Colors.black,
          fontWeight: selected ? FontWeight.bold : FontWeight.normal,
        ),
      ),
      selected: selected,
      onSelected: (_) => onSelected(),
      backgroundColor: Colors.grey[200],
      selectedColor: color,
      checkmarkColor: Colors.white,
      padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
    );
  }
}

class InvoiceCard extends StatefulWidget {
  final Invoice invoice; // Change to use the specific Invoice type
  final bool isLoading;

  const InvoiceCard({
    super.key, 
    required this.invoice, 
    required this.isLoading
  });

  @override
  _InvoiceCardState createState() => _InvoiceCardState();
}

class _InvoiceCardState extends State<InvoiceCard> {
  bool _isExpanded = false;

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.all(8),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(12),
        side: const BorderSide(
          color: Colors.grey,
          width: 3,
        ),
      ),
      child: Column(
        children: [
          Padding(
            padding: EdgeInsets.all(16),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text(
                      'Invoice #${widget.invoice.id}',
                      style: const TextStyle(
                        fontSize: 18,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    _buildPaymentChip(widget.invoice.isPay),
                  ],
                ),
                SizedBox(height: 16),
                Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Invoice Date: ${DateFormat('dd/MM/yyyy').format(widget.invoice.createdOn)}',
                      style: TextStyle(color: Colors.grey[600]),
                    ),
                    SizedBox(height: 8),
                    Text(
                      'Total Amount: ${NumberFormat.currency(locale: 'vi').format(widget.invoice.total)}',
                      style: const TextStyle(
                        fontWeight: FontWeight.bold,
                        color: Colors.green,
                      ),
                    ),
                  ],
                ),
                if (widget.invoice.booking != null) _buildExpandedSection(),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildExpandedSection() {
    return Column(
      children: [
        if (_isExpanded) ..._buildExpandedContent(),
        InkWell(
          onTap: () {
            setState(() {
              _isExpanded = !_isExpanded;
            });
          },
          child: Padding(
            padding: const EdgeInsets.symmetric(vertical: 8),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text(
                  _isExpanded ? 'Thu gọn' : 'Xem chi tiết',
                  style: TextStyle(color: Colors.blue),
                ),
                Icon(
                  _isExpanded 
                    ? Icons.keyboard_arrow_up 
                    : Icons.keyboard_arrow_down,
                  color: Colors.blue,
                ),
              ],
            ),
          ),
        ),
      ],
    );
  }

  List<Widget> _buildExpandedContent() {
    final booking = widget.invoice.booking!;
    return [
      const Divider(height: 24),
      _buildInfoRow('Booking Status:', booking.status),
      _buildInfoRow('Receive Date:', DateFormat('dd/MM/yyyy HH:mm').format(booking.recieveOn)),
      _buildInfoRow('Return Date:', DateFormat('dd/MM/yyyy HH:mm').format(booking.returnOn)),
      _buildInfoRow('Pre-payment:', NumberFormat.currency(locale: 'vi').format(widget.invoice.prePayment)),
      _buildInfoRow('Total Amount:', NumberFormat.currency(locale: 'vi').format(widget.invoice.total)),
      
      if (booking.user != null) ...[
        Divider(height: 24),
        _buildInfoRow('Customer Name:', booking.user!.name),
        if (booking.user!.phoneNumber != null)
          _buildInfoRow('Phone Number:', booking.user!.phoneNumber!),
      ],
    ];
  }

  Widget _buildInfoRow(String label, String value) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
            flex: 2,
            child: Text(
              label,
              style: TextStyle(
                color: Colors.grey[600],
              ),
            ),
          ),
          Expanded(
            flex: 3,
            child: Text(
              value,
              style: const TextStyle(
                fontWeight: FontWeight.bold,
              ),
              textAlign: TextAlign.right,
              overflow: TextOverflow.ellipsis,
              maxLines: 2,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPaymentChip(bool isPay) {
    return Chip(
      label: const Text(
        'Paid',
        style: TextStyle(color: Colors.white),
      ),
      backgroundColor: isPay ? Colors.green : Colors.red,
    );
  }
}