import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class AuthenticationService {
  AuthenticationService();

  Future<bool> isPhoneNumberRegistered(String url) async {
    http.Response response = await http.get(url);
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

  Future<bool> signUp(
      String url, String phoneNumber, String displayName) async {
    var body =
        jsonEncode({'phoneNumber': phoneNumber, 'displayName': displayName});

    http.Response response = await http.post(Uri.encodeFull(url),
        body: body,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        });

    return response.statusCode == 200;
  }
}
