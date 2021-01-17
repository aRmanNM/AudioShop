import 'package:fluttertoast/fluttertoast.dart';
import 'package:http/http.dart' as http;
import 'package:mobile/models/course.dart';
import 'dart:convert';

import 'package:mobile/models/user.dart';

class AuthenticationService {
  AuthenticationService();

  String phoneNumberCheckUrl =
      'https://audioshoppp.ir/api/auth/phoneexists?phoneNumber=';
  String usernameCheckUrl =
      'https://audioshoppp.ir/api/auth/userexists?username=';
  String signUpUrl = 'https://audioshoppp.ir/api/auth/register?role=member';
  String verifyTokenUrl = 'https://audioshoppp.ir/api/auth/verifytoken';
  String refineUserBasketUrl = 'https://audioshoppp.ir/api/user/RefineRepetitiveCourses';
  String getUserCoursesUrl = 'https://audioshoppp.ir/api/user/courses/';
  String verifyPhoneUrl = 'https://audioshoppp.ir/api/auth/verifyphone';

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

  Future<bool> sendVerificationCode(String url, String phoneNumber) async {
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
      var userMap = jsonDecode(data);

      User registeredUser = User.fromJson(userMap);

      return registeredUser;
    }
    else{
      print(response.statusCode);
      return null;
    }
  }

  Future<User> signIn(String phoneNumber, String authToken) async {
    var body =
      jsonEncode({'phoneNumber': phoneNumber, 'authToken': authToken});

    http.Response response = await http.post(Uri.encodeFull(verifyTokenUrl),
        body: body,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        });

    if(response.statusCode == 200){
      String data = response.body;
      var userMap = jsonDecode(data);

      User registeredUser = User.fromJson(userMap);

      return registeredUser;
    }
    else{
      print(response.statusCode);
      return null;
    }
  }

  Future<bool> verifyPhoneNumber(String phoneNumber, String userId) async {
    var body =
      jsonEncode({'phoneNumber': phoneNumber, 'userId': userId});

    http.Response response = await http.post(Uri.encodeFull(verifyPhoneUrl),
        body: body,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        });

    return response.statusCode == 200;
  }

  Future<bool> registerPhoneNumber(String phoneNumber, String authToken, String userId) async {
    var body =
    jsonEncode({'phoneNumber': phoneNumber, 'authToken': authToken, 'userId': userId});

    http.Response response = await http.post(Uri.encodeFull(verifyTokenUrl),
        body: body,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json"
        });

    return response.statusCode == 200;
  }


  Future<List<Course>> refineUserBasket(List<Course> courses, int totalPrice, String userId, String token) async{
    var body = jsonEncode({
      'userId': userId,
      'totalPrice': totalPrice,
      'courseDtos': courses});

    http.Response response = await http.post(Uri.encodeFull(refineUserBasketUrl),
        body: body,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json",
          "Authorization": "Bearer $token",
        });

    if(response.statusCode == 200){
      String data = response.body;
      var courseMap = jsonDecode(data);
      List<Course> coursesList = List<Course>();
      for(var course in courseMap){
        coursesList.add(Course.fromJson(course));
      }
      return coursesList;
    }
    else{
      print(response.statusCode);
      return null;
    }

  }

  Future<List<Course>> getUserCourses(String userId, String token) async {
    http.Response response = await http.get(
        Uri.encodeFull(getUserCoursesUrl + userId),
        headers: {
          "Accept": "application/json",
          "content-type": "application/json",
          "Authorization": "Bearer $token",
        });

    if(response.statusCode == 200){
      String data = response.body;
      var courseMap = jsonDecode(data);
      List<Course> userCoursesList = List<Course>();
      for(var course in courseMap){
        userCoursesList.add(Course.fromJson(course));
      }
      return userCoursesList;
    }
    else{
      print(response.statusCode);
      return null;
    }
  }
}
