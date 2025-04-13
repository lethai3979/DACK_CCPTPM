import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:gowheel_flutterflow_ui/controllers/post_controler.dart';
import 'package:gowheel_flutterflow_ui/pages/admin_vehicle_map.dart';

class AdminVehicleListPage extends StatelessWidget {
  final PostController vehicleController = Get.find<PostController>();

  AdminVehicleListPage({super.key});

  @override
  Widget build(BuildContext context) {
    WidgetsBinding.instance.addPostFrameCallback((_) {
      vehicleController.getAllPosts();
    });

    return Scaffold(
      appBar: AppBar(
        backgroundColor: const Color(0xFF80EE98),
        title: Text(
          'Vehicle Tracking List',
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
      body: Obx(() {
        if (vehicleController.isLoading.value) { 
          return const Center(child: CircularProgressIndicator());
        }
        if (vehicleController.posts.isEmpty) {
          return Center(
            child: Text(
              'No vehicles found.',
              style: GoogleFonts.lexendDeca(fontSize: 16, color: Colors.grey),
            ),
          );
        }

        return ListView.builder(
          padding: const EdgeInsets.all(16),
          itemCount: vehicleController.posts.length,
          itemBuilder: (context, index) {
            final vehicle = vehicleController.posts[index]; 
            final vehicleId = vehicle.id;

            return Card(
              margin: const EdgeInsets.only(bottom: 12),
              elevation: 2,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(8),
              ),
              child: ListTile(
                leading: const Icon(Icons.directions_car, color: Color(0xFF213A58)),
                title: Text(
                  vehicle.name,
                  style: GoogleFonts.lexendDeca(fontWeight: FontWeight.w600),
                ),
                subtitle: Text(
                  'ID: $vehicleId',
                  style: GoogleFonts.lexendDeca(color: Colors.grey[600]),
                ),
                trailing: const Icon(Icons.arrow_forward_ios, size: 16),
                onTap: () {
                  Get.to(() => AdminVehicleMapPage(vehicleId: vehicleId.toString()));
                },
              ),
            );
          },
        );
      }),
    );
  }
}