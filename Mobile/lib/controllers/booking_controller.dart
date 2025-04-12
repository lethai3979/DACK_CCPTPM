import 'package:get/get.dart';
import '../components/snackbar.dart';
import '../models/booking_from_notification_model.dart';
import '../models/booking_model.dart';
import '../models/booking_request_model.dart';
import '../service/booking_service.dart';

class BookingController extends GetxController {
  final BookingService _bookingService = BookingService();
  final RxList<Booking> bookings = <Booking>[].obs;
  final RxList<DateTime> bookedDates = <DateTime>[].obs;
  final RxBool isLoading = false.obs;
  final RxString selectedStatus = ''.obs;
  final Rxn<BookingData> booking = Rxn<BookingData>();

  List<dynamic> get filteredBookings {
    if (selectedStatus.value.isEmpty) {
      return bookings;
    }
    return bookings.where((booking) =>
        booking.status.toLowerCase() == selectedStatus.value.toLowerCase()
    ).toList();
  }

  Future<void> fetchBookings() async {
    try {
      isLoading.value = true;
      final response = await _bookingService.getBookings();
      if (response != null && response.success) {
        bookings.assignAll(response.data);
      }
    } catch (e) {
      Get.snackbar(
        "Error",
        "Failed to fetch bookings.",
        snackPosition: SnackPosition.BOTTOM,
      );
      print('Error fetching bookings: $e');
    } finally {
      isLoading.value = false;
    }
  }

  Future<void> fetchPendingBookingsByUserId() async {
    try {
      isLoading.value = true;
      final response = await _bookingService.getPendingBookingsByUserId();
      if (response != null && response.success) {
        bookings.assignAll(response.data);
      }
    } catch (e) {
      Get.snackbar(
        "Error",
        "Failed to fetch pending bookings.",
        snackPosition: SnackPosition.BOTTOM,
      );
      print('Error fetching pending bookings: $e');
    } finally {
      isLoading.value = false;
    }
  }

  Future<BookingData?> fetchBookingById(int bookingId) async {
    try {
      isLoading.value = true;
      final fetchedBooking = await _bookingService.getBookingById(bookingId);
      if (fetchedBooking != null) {
        booking.value = fetchedBooking;
        return fetchedBooking;
      }
      Snackbar.showError("Error", "Booking not found!");
      return null;
    } catch (e) {
      print('Error fetching booking by ID: $e');
      Snackbar.showError("Error", "Failed to fetch booking!");
      return null;
    } finally {
      isLoading.value = false;
    }
  }

  Future<bool> createBooking(BookingRequest request) async {
    try {
      isLoading.value = true;
      return await _bookingService.createBooking(request);
    } catch (e) {
      Get.snackbar(
        "Error",
        "Failed to create booking.",
        snackPosition: SnackPosition.BOTTOM,
      );
      print('Error creating booking: $e');
      return false;
    } finally {
      isLoading.value = false;
    }
  }

  Future<void> cancelBooking(int bookingId) async {
    try {
      isLoading.value = true;
      final success = await _bookingService.cancelBooking(bookingId.toString());
      if (success) {
        await refreshBookings();
        Snackbar.showSuccess(
          "Success",
          "Booking cancelled successfully!"
        );
      } else {
        Snackbar.showError(
          "Error",
          "Failed to cancel booking."
        );
      }
    } catch (e) {
      Snackbar.showError(
        "Error",
        "An error occurred while cancelling booking."
      );
      print('Error cancelling booking: $e');
    } finally {
      isLoading.value = false;
    }
  }

  Future<void> denyBooking(int bookingId) async {
  try {
    isLoading.value = true;

    // Call the BookingService to deny the booking
    final response = await _bookingService.confirmBooking(
      bookingId,
      id: bookingId,
      isAccept: false,
    );

    if (response['success'] == true) {
      fetchPendingBookingsByUserId();
      Snackbar.showSuccess(
        "Success",
        "Booking denied successfully!"
      );
    } else {
      Snackbar.showError(
        "Error",
        response['message'] ?? "Failed to deny booking."
      );
    }
  } catch (e) {
    Snackbar.showError(
      "Error",
      "An error occurred while denying the booking."
    );
    print('Error denying booking: $e');
  } finally {
    isLoading.value = false;
  }
}

Future<void> acceptBooking(int bookingId) async {
  try {
    isLoading.value = true;

    // Call the BookingService to accept the booking
    final response = await _bookingService.confirmBooking(
      bookingId,
      id: bookingId,
      isAccept: true,
    );

    if (response['success'] == true) {
      fetchPendingBookingsByUserId();
      Snackbar.showSuccess(
        "Success",
        "Booking accepted successfully!"
      );
    } else {
      Snackbar.showError(
        "Error",
        response['message'] ?? "Failed to accept booking."
      );
    }
  } catch (e) {
    Snackbar.showError(
      "Error",
      "An error occurred while accepting the booking."
    );
    print('Error accepting booking: $e');
  } finally {
    isLoading.value = false;
  }
}


  Future<void> refreshBookings() async {
    bookings.clear();
    await fetchBookings();
  }

  void setStatusFilter(String status) {
    selectedStatus.value = status;
  }
}