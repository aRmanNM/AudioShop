import 'package:flutter/material.dart';
import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:mobile/models/course.dart';
import 'package:mobile/services/authentication_service.dart';

class CourseStore extends ChangeNotifier{

  List<Course> _courses = [];
  List<Course> _basket = [];
  Course _currentCourse;
  int _totalBasketPrice = 0;
  List<Course> _userCourses =[];

  String _userId;
  String _userName;
  String _token;
  bool _isLoggedIn = false;

  CourseStore(){
    notifyListeners();
  }

  List<Course> get courses => _courses;
  List<Course> get basket => _basket;
  Course get currentCourse => _currentCourse;
  int get totalBasketPrice => _totalBasketPrice;
  List<Course> get userCourses => _userCourses;

  String get userId => _userId;
  String get userName => _userName;
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

  setTotalBasketPrice(int totalPrice){
    this._totalBasketPrice = totalPrice;
  }

  bool isTokenExpired(String receivedToken){
    _isLoggedIn = JwtDecoder.isExpired(receivedToken);
    return _isLoggedIn;
  }

  Future setUserDetails(String receivedToken) async{

    Map<String, dynamic> decodedToken = JwtDecoder.decode(receivedToken);

    _userId = decodedToken['nameid'];
    _userName = decodedToken['unique_name'];
    _token = receivedToken;

    AuthenticationService authService = AuthenticationService();
    this._userCourses = await authService.getUserCourses(_userId, _token);

    notifyListeners();
  }

  refineUserBasket(List<Course> refinedBasket) {
    this._basket = refinedBasket;
  }
}