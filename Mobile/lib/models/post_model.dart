import 'package:gowheel_flutterflow_ui/models/user_model.dart';

class Post {
  int id;
  bool isDeleted;
  String name;
  String image;
  List<String>? images;
  String description;
  int seat;
  double pricePerHour;
  double pricePerDay;
  int? rideNumber;
  double? avgRating;
  String? carTypeName;
  String? companyName;
  User? user;

  Post({
    required this.id,
    required this.isDeleted,
    required this.name,
    required this.image,
    this.images,
    required this.description,
    required this.seat,
    required this.pricePerHour,
    required this.pricePerDay,
    this.rideNumber,
    this.carTypeName,
    this.companyName,
    this.user,
  });

  factory Post.fromJson(Map<String, dynamic> json) {
    try {
      List<String>? imagesList;
      if (json['images'] != null) {
        imagesList = (json['images'] as List)
            .map((img) => img['url'] as String)
            .toList();
      }

      return Post(
        id: json['id'] as int,
        isDeleted: json['isDeleted'] as bool,
        name: json['name'].toString(),
        image: json['image'].toString(),
        images: imagesList,
        description: json['description'].toString(),
        seat: json['seat'] as int,
        pricePerHour: (json['pricePerHour'] as num).toDouble(),
        pricePerDay: (json['pricePerDay'] as num).toDouble(),
        rideNumber: json['rideNumber'] as int?,
        carTypeName: json['carTypeName'].toString() ?? '',
        companyName: json['companyName'].toString(),
        user: json['user'] != null ? User.fromJson(json['user']) : null,
      );
    } catch (e) {
      rethrow;
    }
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'isDeleted': isDeleted,
      'name': name,
      'image': image,
      'images': images?.map((img) => {'url': img}).toList(),
      'description': description,
      'seat': seat,
      'pricePerHour': pricePerHour,
      'pricePerDay': pricePerDay,
      'rideNumber': rideNumber,
      'carTypeName': carTypeName,
      'companyName': companyName,
      'user': user?.toJson(),
    };
  }
}