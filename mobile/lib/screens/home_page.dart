import 'package:curved_navigation_bar/curved_navigation_bar.dart';
import 'package:firebase_admob/firebase_admob.dart';
import 'package:flutter/material.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:flutter_cache_manager/flutter_cache_manager.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:mobile/models/course.dart';
import 'package:mobile/services/course_service.dart';
import 'package:mobile/store/course_store.dart';
import 'package:provider/provider.dart';

import 'course_page.dart';

class HomePage extends StatefulWidget {
  HomePage(this.courses);
  HomePage.basic();

  dynamic courses;

  @override
  _HomePageState createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  double width = 0;
  double height = 0;
  List<Widget> coursesList = List<Widget>();
  List<Widget> carouselSlider = List<Widget>();
  final String url = 'https://audioshoppp.ir/api/course/';
  DateTime currentBackPressTime;
  Future<dynamic> courses;
  CourseStore courseStore;
  List<Course> courseList = List<Course>();
  int tabIndex = 1;

  @override
  void initState() {
    super.initState();
    courses = getCourses();
  }

  Future<List<Course>> getCourses() async {
    CourseData courseData = CourseData(url);
    courseList = await courseData.getCourses();

    if (courseList != null)
      await updateUI(courseList);
    else
      await updateUI(widget.courses);

    return courseList;
  }

  void goToCoursePage(Course course, var courseCover) async {
    courseStore.setCurrentCourse(course);
    Navigator.push(context, MaterialPageRoute(builder: (context) {
      return CoursePage(course, courseCover);
    }));
  }

  Future updateUI(List<Course> coursesData) async {
    for (var course in coursesData) {
      String picUrl = course.pictureUrl;
      String courseName = course.name;
      String courseDescription = course.description;
      var pictureFile = await DefaultCacheManager().getSingleFile(picUrl);
      coursesList.add(
        Container(
          child: TextButton(
            onPressed: () {
              goToCoursePage(course, pictureFile);
            },
            child: Column(
              children: <Widget>[
                Expanded(
                  flex: 3,
                  child: Padding(
                    padding: const EdgeInsets.only(bottom: 10.0),
                    child: Image.file(
                      //picUrl,
                      pictureFile,
                      fit: BoxFit.fill,
                    ),
                  ),
                ),
                Expanded(
                  flex: 1,
                  child: Text(
                    courseName,
                    overflow: TextOverflow.fade,
                    style: TextStyle(fontSize: 16, color: Colors.white),
                  ),
                ),
              ],
            ),
          ),
        ),
      );

      carouselSlider.add(Card(
        child: TextButton(
          onPressed: () {
            goToCoursePage(course, pictureFile);
          },
          child: Container(
              child: Image.file(
            //picUrl,
            pictureFile,
          )),
        ),
      ));
    }
  }

  Future<bool> onWilPop() async {
    DateTime now = DateTime.now();
    if (currentBackPressTime == null ||
        now.difference(currentBackPressTime) > Duration(seconds: 2)) {
      currentBackPressTime = now;
      Fluttertoast.showToast(msg: 'برای خروج دو بار روی دکمه بازگشت بزنید');
      return Future.value(false);
    }
    return Future.value(true);
  }

  Widget navigationSelect(int tab) {
    if (tab == 0)
      return library();
    else if (tab == 1)
      return home();
    else
      return basket();
  }

  Widget home() {
    return Column(
      children: <Widget>[
        Container(
          height: 20,
        ),
        Expanded(
          flex: 2,
          child: Card(
            child: CarouselSlider(
                options: CarouselOptions(
                    height: height,
                    viewportFraction: 0.6,
                    reverse: false,
                    autoPlay: true,
                    autoPlayInterval: Duration(seconds: 3),
                    autoPlayAnimationDuration: Duration(milliseconds: 800),
                    autoPlayCurve: Curves.fastOutSlowIn,
                    enlargeCenterPage: true),
                items: carouselSlider),
          ),
        ),
        Expanded(
          flex: 3,
          child: Card(
            child: GridView.count(
              padding: const EdgeInsets.all(5),
              crossAxisCount: 3,
              childAspectRatio: (width / height),
              children: coursesList,
            ),
          ),
        ),
      ],
    );
  }

  Widget basket() {
    return Column();
  }

  Widget library() {
    return Column();
  }

  @override
  Widget build(BuildContext context) {
    courseStore = Provider.of<CourseStore>(context);
    courseStore.setAllCourses(courseList);

    FirebaseAdMob.instance
        .initialize(appId: "ca-app-pub-6716792328957551~1144830596")
        .then((value) => myBanner
          ..load()
          ..show(anchorType: AnchorType.bottom));

    width = MediaQuery.of(context).size.width / 2;
    height = (MediaQuery.of(context).size.width / 2) * 1.5;
    return FutureBuilder(
        future: courses,
        builder: (context, data) {
          if (data.hasData)
            return WillPopScope(
                child: Scaffold(
                    bottomNavigationBar: Padding(
                      padding: const EdgeInsets.only(bottom: 59),
                      child: CurvedNavigationBar(
                        color: Color(0xFF1C3C54),
                        buttonBackgroundColor: Color(0xFF386178),
                        animationDuration: Duration(milliseconds: 200),
                        height: 47,
                        backgroundColor: Colors.white,
                        items: <Widget>[
                          Icon(Icons.person, size: 25, color: Colors.white),
                          Icon(Icons.home, size: 25, color: Colors.white),
                          Icon(Icons.shopping_basket,
                              size: 25, color: Colors.white),
                        ],
                        onTap: (index) => {
                          setState(() {
                            tabIndex = index;
                          })
                        },
                        index: 1,
                      ),
                    ),
                    body: navigationSelect(tabIndex)),
                onWillPop: onWilPop);
          else
            return Container(
              color: Colors.white,
              child: SpinKitWave(
                color: Colors.deepOrange[600],
                size: 100.0,
              ),
            );
        });
  }
}

MobileAdTargetingInfo targetingInfo = MobileAdTargetingInfo(
  keywords: <String>['podcast', 'hadi'],
  contentUrl: 'https://flutter.io',
  childDirected: false,
  testDevices: <String>[
    'A36235BD5DAEAA4D6FA305A209159D2A'
  ], // Android emulators are considered test devices
);

BannerAd myBanner = BannerAd(
  // Replace the testAdUnitId with an ad unit id from the AdMob dash.
  // https://developers.google.com/admob/android/test-ads
  // https://developers.google.com/admob/ios/test-ads
  adUnitId: BannerAd.testAdUnitId,
  size: AdSize.fullBanner,
  targetingInfo: targetingInfo,
  listener: (MobileAdEvent event) {
    print("BannerAd event is $event");
  },
);

InterstitialAd myInterstitial = InterstitialAd(
  // Replace the testAdUnitId with an ad unit id from the AdMob dash.
  // https://developers.google.com/admob/android/test-ads
  // https://developers.google.com/admob/ios/test-ads
  adUnitId: InterstitialAd.testAdUnitId,
  targetingInfo: targetingInfo,
  listener: (MobileAdEvent event) {
    print("InterstitialAd event is $event");
  },
);
