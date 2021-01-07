import 'package:flutter/material.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:mobile/services/courses.dart';

import 'course_page.dart';

class HomePage extends StatefulWidget {
  HomePage(this.courses);

  final courses;

  @override
  _HomePageState createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  List<Widget> coursesList = List<Widget>();
  List<Widget> carouselSlider = List<Widget>();
  final String url = 'https://audioshoppp.ir/api/course/episodes/';

  @override
  void initState() {
    super.initState();
    updateUI(widget.courses);
  }

  void updateUI(dynamic coursesData) {
    for (var course in coursesData) {
      String picUrl = course['pictureUrl'];
      String courseName = course['name'];
      String courseDescription = course['description'];
      coursesList.add(
        Container(
          child: TextButton(
            onPressed: () {
              goToCoursePage(course);
            },
            child: Column(
              children: <Widget>[
                Padding(
                  padding: const EdgeInsets.only(bottom: 10.0),
                  child: Image.network(
                    picUrl,
                    fit: BoxFit.fill,
                  ),
                ),
                Text(
                  courseName,
                  style: TextStyle(fontSize: 16),
                ),
                Text(courseDescription, overflow: TextOverflow.ellipsis),
              ],
            ),
          ),
        ),
      );

      carouselSlider.add(TextButton(
        onPressed: () {
          goToCoursePage(course);
        },
        child: Container(
            margin: EdgeInsets.symmetric(horizontal: 2.0),
            child: Image.network(
                picUrl,
            )),
      ));
    }
  }

  void goToCoursePage(dynamic course) async{

    String url = this.url + course['id'].toString();
    CourseData courseEpisodeData = CourseData(url);
    dynamic courseEpisodes = await courseEpisodeData.getData();

    Navigator.push(context, MaterialPageRoute(builder: (context) {
      return CoursePage(course, courseEpisodes);
    }));
  }

  @override
  Widget build(BuildContext context) {
    double width = MediaQuery.of(context).size.width / 2;
    double height = (MediaQuery.of(context).size.width / 2) * 1.5;
    return Scaffold(
      body: Column(
        children: <Widget>[
          Expanded(
            child: CarouselSlider(
                options: CarouselOptions(
                  height: height,
                  enlargeCenterPage: true,
                  viewportFraction: 0.7,
                ),
                items: carouselSlider),
          ),
          Expanded(
            child: GridView.count(
              padding: const EdgeInsets.all(20),
              crossAxisSpacing: 20,
              crossAxisCount: 2,
              childAspectRatio: (width / height),
              children: coursesList,
            ),
          ),
        ],
      ),
    );
  }
}
