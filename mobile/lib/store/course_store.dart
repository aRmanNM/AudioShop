import 'package:flutter/cupertino.dart';
import 'package:mobile/models/course.dart';

class CourseStore extends ChangeNotifier{

  List<Course> courses = [];
  List<Course> basket = [];
  Course currentCourse;
  CourseStore(){
    notifyListeners();
  }
}