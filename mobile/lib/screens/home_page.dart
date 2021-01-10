import 'package:curved_navigation_bar/curved_navigation_bar.dart';
import 'package:firebase_admob/firebase_admob.dart';
import 'package:flutter/material.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:mobile/services/courses.dart';

import 'course_page.dart';
import 'loading_screen.dart';

class HomePage extends StatefulWidget {
  HomePage(this.courses);
  HomePage.basic();

  dynamic courses;

  @override
  _HomePageState createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  List<Widget> coursesList = List<Widget>();
  List<Widget> carouselSlider = List<Widget>();
  final String url = 'https://audioshoppp.ir/api/course/';
  DateTime currentBackPressTime;
  Future<dynamic> courses;

  @override
  void initState() {
    super.initState();
    courses = getCourses();
  }

  Future<dynamic> getCourses() async {
    CourseData courseData = CourseData(url);
    var course = await courseData.getData();

    if(course != null)
      updateUI(course);
    else
      updateUI(widget.courses);

    return course;
  }

  void goToCoursePage(dynamic course) async {
    Navigator.push(context, MaterialPageRoute(builder: (context) {
      return CoursePage(course);
    }));
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
                  style: TextStyle(
                      fontSize: 12,
                      color: Colors.black),
                ),
                Text(
                    courseDescription,
                    overflow: TextOverflow.ellipsis,
                    style: TextStyle(
                        fontSize: 10,
                      color: Colors.black26
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
            goToCoursePage(course);
          },
          child: Container(
              child: Image.network(
                picUrl,
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

  @override
  Widget build(BuildContext context) {
    FirebaseAdMob.instance.initialize(appId: "ca-app-pub-6716792328957551~1144830596")
      .then((value) => myBanner..load()..show(anchorType: AnchorType.bottom));

    double width = MediaQuery.of(context).size.width / 2;
    double height = (MediaQuery.of(context).size.width / 2) * 1.5;
    return FutureBuilder(
        future: courses,
        builder: (context, data){
          if(data.hasData)
            return WillPopScope(
                child: Scaffold(
                  bottomNavigationBar: Padding(
                    padding: const EdgeInsets.only(bottom: 59),
                    child: CurvedNavigationBar(
                      animationDuration: Duration(milliseconds: 200),
                      height: 47,
                      backgroundColor: Colors.deepOrange[200],
                      items: <Widget>[
                        Icon(Icons.person, size: 25, color: Colors.deepOrange[600]),
                        Icon(Icons.home, size: 25, color: Colors.deepOrange[600]),
                        Icon(Icons.shopping_basket, size: 25, color: Colors.deepOrange[600]),
                      ],
                      onTap: (index) => {
                        debugPrint('current index is $index')
                      },
                      index: 1,
                    ),
                  ),
                  body: Column(
                    children: <Widget>[
                      Container(height: 20,),
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
                                enlargeCenterPage: true
                              ),
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
                  ),
                ),
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
  testDevices: <String>['A36235BD5DAEAA4D6FA305A209159D2A'], // Android emulators are considered test devices
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