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
  bool _hasPhoneNumber = false;

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
  bool get hasPhoneNumber => _hasPhoneNumber;

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

  Future setUserDetails(String receivedToken, bool hasPhoneNumber) async{

    Map<String, dynamic> decodedToken = JwtDecoder.decode(receivedToken);

    if(receivedToken != ""){
      _userId = decodedToken['nameid'];
      _userName = decodedToken['unique_name'];
      AuthenticationService authService = AuthenticationService();
      _userCourses = await authService.getUserCourses(_userId, receivedToken);
    }
    else{
      _userId = '';
      _userName = '';
      _userCourses.clear();
    }

    _hasPhoneNumber = hasPhoneNumber;
    _token = receivedToken;

  }

  refineUserBasket(List<Course> refinedBasket) {
    if(refinedBasket.isNotEmpty && refinedBasket.length > 0)
      this._basket = refinedBasket;
    else
      this._basket.clear();
  }
}