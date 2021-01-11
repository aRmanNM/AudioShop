import 'package:flutter/cupertino.dart';
import 'package:mobile/models/course.dart';

class CourseStore extends ChangeNotifier{

  List<Course> _courses = [];
  List<Course> _basket = [];
  Course _currentCourse;

  CourseStore(){
    notifyListeners();
  }

  List<Course> get courses => _courses;
  List<Course> get basket => _basket;
  Course get currentCourse => _currentCourse;

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
      basket.add(toBeAddedCourse);
      notifyListeners();
      return true;
    }
    return false;
  }

  deleteCourseFromBasket(Course toBeDeletedCourse){
    basket.remove(toBeDeletedCourse);
    notifyListeners();
  }
}