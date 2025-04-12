import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:gowheel_flutterflow_ui/pages/list_booking.dart';
import 'package:gowheel_flutterflow_ui/pages/list_favorite.dart';
import 'package:gowheel_flutterflow_ui/pages/list_invoice.dart';
import 'package:gowheel_flutterflow_ui/pages/list_post.dart';

class QuickActionGrid extends StatelessWidget {
  const QuickActionGrid({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 20),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Text(
            "Quick Actions",
            style: TextStyle(
                      fontSize: 22,
                      fontWeight: FontWeight.bold,
                      color: Colors.black,
            )
          ),
          const SizedBox(height: 15),
          GridView.count(
            shrinkWrap: true,
            physics: const NeverScrollableScrollPhysics(),
            crossAxisCount: 4,
            crossAxisSpacing: 10,
            mainAxisSpacing: 10,
            // Set a fixed height for each item
            childAspectRatio: 1, // This makes each grid item square
            children: [
              _buildActionItem(
                icon: Icons.directions_car,
                label: "All Cars",
                color: Colors.blue,
                onTap: () => Get.to(() => const AvailableCarsPage()),
              ),
              _buildActionItem(
                icon: Icons.favorite,
                label: "Favorites",
                color: Colors.red,
                onTap: () => Get.to(() => FavoritePage()),
              ),
              _buildActionItem(
                icon: Icons.car_rental,
                label: "Your Booking",
                color: Colors.purple,
                onTap: () => Get.to(() => BookingScreen()),
              ),
              _buildActionItem(
                icon: Icons.offline_pin_sharp,
                label: "Your Invoice",
                color: Colors.orange,
                onTap: () => Get.to(() => const InvoiceScreen()),
              ),
              _buildActionItem(
                icon: Icons.support_agent,
                label: "Support",
                color: Colors.indigo,
                onTap: () {
                  // Handle support action
                },
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildActionItem({
    required IconData icon,
    required String label,
    required Color color,
    required VoidCallback onTap,
  }) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        constraints: const BoxConstraints(
          minHeight: 50,  // Set minimum height
          maxHeight: 80,  // Set maximum height
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,  // Use minimum space needed
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Container(
              padding: const EdgeInsets.all(8),
              decoration: BoxDecoration(
                color: color.withOpacity(0.1),
                borderRadius: BorderRadius.circular(12),
              ),
              child: Icon(
                icon,
                color: color,
                size: 24,  // Reduced icon size
              ),
            ),
            const SizedBox(height: 4),  // Reduced spacing
            Text(
              label,
              style: const TextStyle(
                fontSize: 11,  // Reduced font size
                color: Colors.black87,
              ),
              textAlign: TextAlign.center,
              maxLines: 1,  // Limit to one line
              overflow: TextOverflow.ellipsis,  // Handle text overflow
            ),
          ],
        ),
      ),
    );
  }
}