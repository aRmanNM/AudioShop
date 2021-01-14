import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

import 'package:mobile/models/user.dart';

class AuthenticationService {
  AuthenticationService();

  String phoneNumberCheckUrl =
      'https://audioshoppp.ir/api/auth/phoneexists?phoneNumber=';
  String usernameCheckUrl =
      'https://audioshoppp.ir/api/auth/userexists?username=';
  String signUpUrl = '/api/auth/register?role=member';

  Future<bool> isPhoneNumberRegistered(String phoneNumber) async {
    http.Response response = await http.get(phoneNumberCheckUrl + phoneNumber);
    return await responseChecker(response);
  }

  Future<bool> usernameExists(String username) async {
    http.Response response = await http.get(usernameCheckUrl + username);
    return await responseChecker(response);
  }

  Future<bool> responseChecker(http.Response response) async{
    if (response.statusCode == 200) {
      bool data = response.body.toLowerCase() == 'true';
      return data;
    } else {
      //TODO return correct answer
      print(response.statusCode);
      return null;
    }
  }

  Future<bool> signIn(String url, String phoneNumber) async {
    var body = jsonEncode({'phoneNumber': phoneNumber});

    http.Response response = await http.post(Uri.encodeFull(url),
        body: body,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        });

    return response.statusCode == 200;
  }

  Future<User> signUp(String userName, String password) async {
    var body =
        jsonEncode({'userName': userName, 'password': password});

    http.Response response = await http.post(Uri.encodeFull(signUpUrl),
        body: body,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        });
    if(response.statusCode == 200){
      String data = response.body;
      var courseMap = jsonDecode(data);

      User registeredUser = User.fromJson(courseMap);

      return registeredUser;
    }
    else{
      print(response.statusCode);
      return null;
    }
  }
}
