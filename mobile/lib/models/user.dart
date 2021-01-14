class User{

  String token;
  bool hasPhoneNumber;

  User({this.token, this.hasPhoneNumber});

  User.fromJson(Map<String, dynamic> json)
      : token = json['token'],
        hasPhoneNumber = json['hasPhoneNumber'];

  Map<String, dynamic> toJson() => {
    'token': token,
    'hasPhoneNumber': hasPhoneNumber,
  };
}