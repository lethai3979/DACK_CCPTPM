import 'dart:convert';
import 'package:gowheel_flutterflow_ui/models/post_model.dart';
import 'package:gowheel_flutterflow_ui/service/storage_service.dart';
import 'package:gowheel_flutterflow_ui/url.dart';
import 'package:http/http.dart' as http;

class PostService {
  static final PostService _instance = PostService._internal();
  factory PostService() => _instance;
  PostService._internal();

  final TokenService tokenService = TokenService();

  Future<List<Post>> getAllPosts() async {
    try {
      final response = await http.get(
        Uri.parse("${URL.baseUrl}Admin/Post/GetAll"),
        headers: {
          'Content-Type': 'application/json',
        },
      );

      if (response.statusCode == 200) {
        final Map<String, dynamic> responseData = json.decode(response.body);

        if (responseData['success'] != true) {
          return [];
        }
        
        if (responseData['data'] == null) {
          return [];
        }

        final List<dynamic> postsJson = responseData['data']['listPost'] as List<dynamic>;
        return postsJson
            .map((json) => Post.fromJson(json))
            .whereType<Post>()
            .toList();
      } else {
        return[];
      }
    } catch (e) {
      print(e);
      rethrow;
    }
  }

  
  Future<bool> addPost({
    required String name,
    required String description,
    required int seat,
    required String rentLocation,
    required bool hasDriver,
    required double pricePerHour,
    required double pricePerDay,
    required bool gear,
    required String fuel,
    required double fuelConsumed,
    required int carTypeId,
    required int companyId,
    required List<int> amenitiesIds,
    required String imagePath,
    required List<String> imagesList,
  }) async {
    try {
      final token = await tokenService.getToken();
      var uri = Uri.parse("${URL.baseUrl}Admin/Post/Add");

      var request = http.MultipartRequest('POST', uri);

      // Add headers
      request.headers.addAll({
        'Authorization': 'Bearer $token',
        'accept': 'text/plain',
      });

      // Add file
      var imageFile = await http.MultipartFile.fromPath('Image', imagePath);
      request.files.add(imageFile);

      // Add multiple images
      for (var imagePath in imagesList) {
        var imageFile = await http.MultipartFile.fromPath('ImagesList', imagePath);
        request.files.add(imageFile);
      }

      // Add other fields
      request.fields.addAll({
        'Name': name,
        'Description': description,
        'Seat': seat.toString(),
        'RentLocation': rentLocation,
        'HasDriver': hasDriver.toString(),
        'PricePerHour': pricePerHour.toString(),
        'PricePerDay': pricePerDay.toString(),
        'Gear': gear.toString(),
        'Fuel': fuel,
        'FuelConsumed': fuelConsumed.toString(),
        'CarTypeId': carTypeId.toString(),
        'CompanyId': companyId.toString(),
      });

      // Add amenities
      for (var i = 0; i < amenitiesIds.length; i++) {
        request.fields['AmenitiesIds[$i]'] = amenitiesIds[i].toString();
      }

      var response = await request.send();
      var responseData = await response.stream.bytesToString();
      print(responseData);

      if (response.statusCode == 200) {
        final decodedResponse = json.decode(responseData);
        return decodedResponse['success'] ?? false;
      } else {
        throw Exception('Failed to add post: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Failed to add post: $e');
    }
  }

  Future<List<Post>> loadMorePosts(int pageIndex) async {
    try {
      final response = await http.get(
        Uri.parse("${URL.baseUrl}Admin/Post/GetAll?pageIndex=$pageIndex"),
        headers: {
          'Content-Type': 'application/json',
        },
      );
      print(Uri.parse("${URL.baseUrl}User/Post/GetAll?page=$pageIndex").toString());
      if (response.statusCode == 200) {
        final Map<String, dynamic> responseData = json.decode(response.body);

        if (responseData['success'] != true) {
          return [];
        }
        
        if (responseData['data'] == null) {
          return [];
        }
        print("Response for page $pageIndex: ${response.body}");
        final List<dynamic> postsJson = responseData['data']['listPost'] as List<dynamic>;
        return postsJson
            .map((json) => Post.fromJson(json))
            .whereType<Post>()
            .toList();
      } else {
        return[];
      }
    } catch (e) {
      print(e);
      rethrow;
    }
  }

  
}