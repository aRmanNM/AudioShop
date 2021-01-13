import 'package:flutter/material.dart';
import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:mobile/models/course.dart';

class CourseStore extends ChangeNotifier{

  List<Course> _courses = [];
  List<Course> _basket = [];
  Course _currentCourse;

  String _userId;
  String _displayName;
  String _token;
  bool _isLoggedIn = false;

  CourseStore(){
    notifyListeners();
  }

  List<Course> get courses => _courses;
  List<Course> get basket => _basket;
  Course get currentCourse => _currentCourse;

  String get userId => _userId;
  String get displayName => _displayName;
  String get token => _token;
  bool get isLoggedIn => _isLoggedIn;

  setAllCourses(List<Course> allCourses){
    this._courses = allCourses;
  }

  setCurrentCourse(Course tapedCourse){
    this._currentCourse = tapedCourse;
  }

  bool addCourseToBasket(Course toBeAddedCourse){
    Course similarCourse = _basket
        .firstWhere((x) => x.id == toBeAddedCourse.id, orElse: () => null);

    if(similarCourse == null) {
      _basket.add(toBeAddedCourse);
      notifyListeners();
      return true;
    }
    return false;
  }

  deleteCourseFromBasket(Course toBeDeletedCourse){
    _basket.remove(toBeDeletedCourse);
    notifyListeners();
  }

  bool isTokenExpired(String receivedToken){
    _isLoggedIn = JwtDecoder.isExpired(receivedToken);
    return _isLoggedIn;
  }

  setUserDetails(String receivedToken){

    Map<String, dynamic> decodedToken = JwtDecoder.decode(receivedToken);

    _userId = decodedToken['nameid'];
    _displayName = decodedToken['given_name'];
    _token = receivedToken;

    notifyListeners();
  }
}