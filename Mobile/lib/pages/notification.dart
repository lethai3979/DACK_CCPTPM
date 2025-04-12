import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:gowheel_flutterflow_ui/pages/booking_notification_detail.dart';
import 'package:timeago/timeago.dart' as timeago;
import '../controllers/booking_controller.dart';
import '../controllers/notification_controller.dart';

class NotificationView extends StatelessWidget {
  final NotificationController _controller = Get.put(NotificationController());

  NotificationView({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: const Color(0xFF80EE98),
        automaticallyImplyLeading: false,
        title: Text(
          'Your Notification',
          style: GoogleFonts.lexendDeca(
            color: const Color(0xFF213A58),
            fontSize: 20.0,
            fontWeight: FontWeight.bold,
            letterSpacing: 0.0,
          ),
        ),
      ),
      body: Obx(() {
        return RefreshIndicator(
          onRefresh: () async {
            await _controller.fetchNotifications();
          },
          child: _controller.isLoading.value
              ? const Center(child: CircularProgressIndicator())
              : (_controller.notifications.isEmpty
                  ? ListView(
                      children: [
                        Center(
                          child: Column(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: [
                              const SizedBox(height: 16),
                              Text(
                                "No notifications yet.",
                                style: TextStyle(
                                  fontSize: 18,
                                  color: Colors.grey[600],
                                ),
                              ),
                              const SizedBox(height: 8),
                              Text(
                                "Your notifications will be displayed here.",
                                style: TextStyle(
                                  color: Colors.grey[500],
                                ),
                              ),
                            ],
                          ),
                        ),
                      ],
                    )
                  : ListView.builder(
                      itemCount: _controller.notifications.length,
                      itemBuilder: (context, index) {
                        final notification = _controller.notifications[index];
                        return ListTile(
                          title: Text(notification.content ?? 'No Content'),
                          subtitle: Text(
                            notification.createOn != null
                                ? timeago.format(DateTime.parse(notification.createOn!).toLocal())
                                : 'No Date',
                          ),
                          trailing: notification.isRead == false
                              ? const Icon(Icons.circle, color: Colors.blue, size: 10)
                              : null,
                          onTap: () async {
                            if (notification.bookingId != null) {
                              try {
                                _controller.markNotificationRead(notification.id!);
                                final BookingController bookingController = Get.put(BookingController());
                                final booking = await bookingController.fetchBookingById(notification.bookingId!);
                                if (booking != null) {
                                  Get.to(() => BookingFromNotification(booking: booking));
                                } else {
                                  Get.snackbar(
                                    'Error',
                                    'Booking not found.',
                                    snackPosition: SnackPosition.BOTTOM,
                                  );
                                }
                              } catch (e) {
                                print(e);
                                Get.snackbar(
                                  'Error',
                                  'Failed to fetch booking details. Please try again later.',
                                  snackPosition: SnackPosition.BOTTOM,
                                );
                              }
                            } else {
                              Get.snackbar(
                                'No Booking',
                                'This notification is not associated with a booking.',
                                snackPosition: SnackPosition.BOTTOM,
                              );
                            }
                          },
                        );
                      },
                    )),
        );
      }),
    );
  }
}
