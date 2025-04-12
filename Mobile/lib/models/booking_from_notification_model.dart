class BookingResponse1 {
  final bool success;
  final String? message;
  final int statusCode;
  final BookingData? data;

  BookingResponse1({
    required this.success,
    this.message,
    required this.statusCode,
    this.data,
  });

  factory BookingResponse1.fromJson(Map<String, dynamic> json) {
    return BookingResponse1(
      success: json['success'] ?? false,
      message: json['message'],
      statusCode: json['statusCode'] ?? 0,
      data: json['data'] != null ? BookingData.fromJson(json['data']) : null,
    );
  }
}

class BookingData {
  final double? prePayment;
  final double? total;
  final double? finalValue;
  final DateTime? recieveOn;
  final DateTime? returnOn;
  final String? status;
  final bool? isRequest;
  final bool? isResponse;
  final String? longitude;
  final String? latitude;
  final bool? isRequireDriver;
  final bool? hasDriver;
  final bool? isPay;
  final UserData? user;
  final PostData? post;
  final PromotionData? promotion;
  final DriverData? driver;
  final int? id;
  final String? createdById;
  final DateTime? createdOn;
  final String? modifiedById;
  final DateTime? modifiedOn;
  final bool? isDeleted;

  BookingData({
    this.prePayment,
    this.total,
    this.finalValue,
    this.recieveOn,
    this.returnOn,
    this.status,
    this.isRequest,
    this.isResponse,
    this.longitude,
    this.latitude,
    this.isRequireDriver,
    this.hasDriver,
    this.isPay,
    this.user,
    this.post,
    this.promotion,
    this.driver,
    this.id,
    this.createdById,
    this.createdOn,
    this.modifiedById,
    this.modifiedOn,
    this.isDeleted,
  });

  factory BookingData.fromJson(Map<String, dynamic> json) {
    return BookingData(
      prePayment: json['prePayment']?.toDouble(),
      total: json['total']?.toDouble(),
      finalValue: json['finalValue']?.toDouble(),
      recieveOn: json['recieveOn'] != null ? DateTime.parse(json['recieveOn']) : null,
      returnOn: json['returnOn'] != null ? DateTime.parse(json['returnOn']) : null,
      status: json['status'],
      isRequest: json['isRequest'],
      isResponse: json['isResponse'],
      longitude: json['longitude'],
      latitude: json['latitude'],
      isRequireDriver: json['isRequireDriver'],
      hasDriver: json['hasDriver'],
      isPay: json['isPay'],
      user: json['user'] != null ? UserData.fromJson(json['user']) : null,
      post: json['post'] != null ? PostData.fromJson(json['post']) : null,
      promotion: json['promotion'] != null ? PromotionData.fromJson(json['promotion']) : null,
      driver: json['driver'] != null ? DriverData.fromJson(json['driver']) : null,
      id: json['id'],
      createdById: json['createdById'],
      createdOn: json['createdOn'] != null ? DateTime.parse(json['createdOn']) : null,
      modifiedById: json['modifiedById'],
      modifiedOn: json['modifiedOn'] != null ? DateTime.parse(json['modifiedOn']) : null,
      isDeleted: json['isDeleted'],
    );
  }
}

class UserData {
  final String? id;
  final String? name;
  final String? license;
  final DateTime? createDate;
  final String? phoneNumber;
  final String? email;
  final String? cic;
  final String? image;
  final String? longitude;
  final String? latitude;
  final bool? isSubmitDriver;
  final bool? lockoutEnabled;
  final DateTime? lockoutEnd;
  final int? reportPoint;
  final DateTime? birthday;
  final List<String>? roles;

  UserData({
    this.id,
    this.name,
    this.license,
    this.createDate,
    this.phoneNumber,
    this.email,
    this.cic,
    this.image,
    this.longitude,
    this.latitude,
    this.isSubmitDriver,
    this.lockoutEnabled,
    this.lockoutEnd,
    this.reportPoint,
    this.birthday,
    this.roles,
  });

  factory UserData.fromJson(Map<String, dynamic> json) {
    return UserData(
      id: json['id'],
      name: json['name'],
      license: json['license'],
      createDate: json['createDate'] != null ? DateTime.parse(json['createDate']) : null,
      phoneNumber: json['phoneNumber'],
      email: json['email'],
      cic: json['cic'],
      image: json['image'],
      longitude: json['longitude'],
      latitude: json['latitude'],
      isSubmitDriver: json['isSubmitDriver'],
      lockoutEnabled: json['lockoutEnabled'],
      lockoutEnd: json['lockoutEnd'] != null ? DateTime.parse(json['lockoutEnd']) : null,
      reportPoint: json['reportPoint'],
      birthday: json['birthday'] != null ? DateTime.parse(json['birthday']) : null,
      roles: json['roles'] != null ? List<String>.from(json['roles']) : null,
    );
  }
}

class PostData {
  final String? name;
  final String? image;
  final List<String>? images;
  final String? description;
  final int? seat;
  final String? rentLocation;
  final bool? hasDriver;
  final double? pricePerHour;
  final double? pricePerDay;
  final bool? gear;
  final String? fuel;
  final double? fuelConsumed;
  final int? rideNumber;
  final double? avgRating;
  final bool? isAvailable;
  final bool? isDisabled;
  final String? carTypeName;
  final String? companyName;
  final UserData? user;
  final List<dynamic>? postAmenities;
  final List<dynamic>? postPromotions;
  final List<dynamic>? ratings;
  final int? id;
  final String? createdById;
  final DateTime? createdOn;
  final String? modifiedById;
  final DateTime? modifiedOn;
  final bool? isDeleted;

  PostData({
    this.name,
    this.image,
    this.images,
    this.description,
    this.seat,
    this.rentLocation,
    this.hasDriver,
    this.pricePerHour,
    this.pricePerDay,
    this.gear,
    this.fuel,
    this.fuelConsumed,
    this.rideNumber,
    this.avgRating,
    this.isAvailable,
    this.isDisabled,
    this.carTypeName,
    this.companyName,
    this.user,
    this.postAmenities,
    this.postPromotions,
    this.ratings,
    this.id,
    this.createdById,
    this.createdOn,
    this.modifiedById,
    this.modifiedOn,
    this.isDeleted,
  });

  factory PostData.fromJson(Map<String, dynamic> json) {
    return PostData(
      name: json['name'],
      image: json['image'],
      images: json['images'] != null ? List<String>.from(json['images']) : null,
      description: json['description'],
      seat: json['seat'],
      rentLocation: json['rentLocation'],
      hasDriver: json['hasDriver'],
      pricePerHour: json['pricePerHour']?.toDouble(),
      pricePerDay: json['pricePerDay']?.toDouble(),
      gear: json['gear'],
      fuel: json['fuel'],
      fuelConsumed: json['fuelConsumed']?.toDouble(),
      rideNumber: json['rideNumber'],
      avgRating: json['avgRating']?.toDouble(),
      isAvailable: json['isAvailable'],
      isDisabled: json['isDisabled'],
      carTypeName: json['carTypeName'],
      companyName: json['companyName'],
      user: json['user'] != null ? UserData.fromJson(json['user']) : null,
      postAmenities: json['postAmenities'],
      postPromotions: json['postPromotions'],
      ratings: json['ratings'],
      id: json['id'],
      createdById: json['createdById'],
      createdOn: json['createdOn'] != null ? DateTime.parse(json['createdOn']) : null,
      modifiedById: json['modifiedById'],
      modifiedOn: json['modifiedOn'] != null ? DateTime.parse(json['modifiedOn']) : null,
      isDeleted: json['isDeleted'],
    );
  }
}

class PromotionData {
  final String? content;
  final double? discountValue;
  final DateTime? expiredDate;
  final bool? isAdminPromotion;
  final int? id;
  final String? createdById;
  final DateTime? createdOn;
  final String? modifiedById;
  final DateTime? modifiedOn;
  final bool? isDeleted;

  PromotionData({
    this.content,
    this.discountValue,
    this.expiredDate,
    this.isAdminPromotion,
    this.id,
    this.createdById,
    this.createdOn,
    this.modifiedById,
    this.modifiedOn,
    this.isDeleted,
  });

  factory PromotionData.fromJson(Map<String, dynamic> json) {
    return PromotionData(
      content: json['content'],
      discountValue: json['discountValue']?.toDouble(),
      expiredDate: json['expiredDate'] != null ? DateTime.parse(json['expiredDate']) : null,
      isAdminPromotion: json['isAdminPromotion'],
      id: json['id'],
      createdById: json['createdById'],
      createdOn: json['createdOn'] != null ? DateTime.parse(json['createdOn']) : null,
      modifiedById: json['modifiedById'],
      modifiedOn: json['modifiedOn'] != null ? DateTime.parse(json['modifiedOn']) : null,
      isDeleted: json['isDeleted'],
    );
  }
}

class DriverData {
  final UserData? user;
  final double? ratingPoint;
  final double? pricePerHour;

  DriverData({
    this.user,
    this.ratingPoint,
    this.pricePerHour,
  });

  factory DriverData.fromJson(Map<String, dynamic> json) {
    return DriverData(
      user: json['user'] != null ? UserData.fromJson(json['user']) : null,
      ratingPoint: json['ratingPoint']?.toDouble(),
      pricePerHour: json['pricePerHour']?.toDouble(),
    );
  }
}