import 'package:curved_navigation_bar/curved_navigation_bar.dart';
import 'package:firebase_admob/firebase_admob.dart';
import 'package:flutter/material.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:flutter_cache_manager/flutter_cache_manager.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:intl/intl.dart';
import 'package:mobile/models/course.dart';
import 'package:mobile/screens/checkout_page.dart';
import 'package:mobile/screens/login_page.dart';
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
  final secureStorage = FlutterSecureStorage();
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
  bool delete = false;
  double totalBasketPrice = 0;

  @override
  void initState() {
    super.initState();
    courses = getCourses();
    loginStatement();
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
                    child: ClipRRect(
                      borderRadius: BorderRadius.circular(10),
                      child: Image.file(
                        //picUrl,
                        pictureFile,
                        fit: BoxFit.fill,
                      ),
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

      carouselSlider.add(TextButton(
        onPressed: () {
          goToCoursePage(course, pictureFile);
        },
        child: Container(
            child: ClipRRect(
          borderRadius: BorderRadius.circular(10),
          child: Image.file(
            //picUrl,
            pictureFile,
          ),
        )),
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
    totalBasketPrice = 0;
    return courseStore.basket.length > 0
        ? Column(
            children: <Widget>[
              Expanded(
                flex: 8,
                child: ListView.builder(
                    itemCount: courseStore.basket.length,
                    itemBuilder: (BuildContext context, int index) {
                      return Padding(
                        padding: const EdgeInsets.symmetric(
                            vertical: 2, horizontal: 8),
                        child: Card(
                          elevation: 8,
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(15.0),
                          ),
                          child: IntrinsicHeight(
                            child: Row(
                              crossAxisAlignment: CrossAxisAlignment.stretch,
                              children: <Widget>[
                                Expanded(
                                    flex: 2,
                                    child: ClipRRect(
                                      borderRadius: BorderRadius.circular(15),
                                      child: Image.network(
                                          courseStore.basket[index].pictureUrl),
                                    )),
                                Expanded(
                                  flex: 6,
                                  child: Column(
                                    mainAxisAlignment:
                                        MainAxisAlignment.spaceEvenly,
                                    children: [
                                      Text(
                                        courseStore.basket[index].name,
                                        style: TextStyle(fontSize: 19),
                                      ),
                                      Text(
                                        NumberFormat('#,###').format(courseStore
                                                .basket[index].price) +
                                            ' تومان',
                                        style: TextStyle(fontSize: 15),
                                      ),
                                    ],
                                  ),
                                ),
                                Expanded(
                                  flex: 1,
                                  child: ClipRRect(
                                    borderRadius: BorderRadius.only(
                                      topLeft: Radius.circular(15),
                                      bottomLeft: Radius.circular(15),
                                    ),
                                    child: Container(
                                      color: Colors.red,
                                      child: TextButton(
                                        child: Icon(Icons.delete_outline_sharp,
                                            size: 25, color: Colors.white),
                                        onPressed: () {
                                          showAlertDialog(context,
                                              courseStore.basket[index]);
                                        },
                                      ),
                                    ),
                                  ),
                                )
                              ],
                            ),
                          ),
                        ),
                      );
                    }),
              ),
              Expanded(
                flex: 3,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: <Widget>[
                    Expanded(
                      flex: 2,
                      child: Padding(
                        padding: const EdgeInsets.symmetric(
                            vertical: 0, horizontal: 15.0),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: <Widget>[
                            Padding(
                              padding: const EdgeInsets.only(right: 5),
                              child: Text('مجموع دوره های انتخاب شده: '),
                            ),
                            Card(
                                color: Color(0xFF202028),
                                shape: RoundedRectangleBorder(
                                  borderRadius: BorderRadius.circular(15.0),
                                ),
                                child: Padding(
                                  padding: const EdgeInsets.fromLTRB(8,8,8,8),
                                  child: Text(basketPrice(),),
                                )),
                          ],
                        ),
                      ),
                    ),
                    Expanded(
                      flex: 3,
                      child: Padding(
                        padding: const EdgeInsets.fromLTRB(15,0,15,20),
                        child: Card(
                          color: Color(0xFF20BFA9),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(15.0),
                          ),
                          child: TextButton(
                            onPressed: (){
                              if(courseStore.isLoggedIn)
                                Navigator.push(context, MaterialPageRoute(builder: (context){
                                  return CheckOutPage();
                                }));
                              else{
                                Navigator.push(context, MaterialPageRoute(builder: (context){
                                  return LoginPage();
                                }));
                              }
                            },
                            child: Center(
                              child: Text(
                                  'ادامه خرید',
                                  style: TextStyle(
                                      fontSize: 23,
                                      color: Colors.white,
                                  ),
                              ),
                            ),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              )
            ],
          )
        : Center(
            child: Text('دوره ای در سبد خرید شما موجود نمی باشد'),
          );
  }

  String basketPrice() {
    for (var course in courseStore.basket) totalBasketPrice += course.price;

    return NumberFormat('#,###').format(totalBasketPrice) + ' تومان';
  }

  Widget library() {
    return Column();
  }

  showAlertDialog(BuildContext context, Course course) {
    // set up the buttons
    Widget cancelButton = FlatButton(
      child: Text("خیر"),
      onPressed: () {
        setState(() {
          Navigator.of(context).pop();
        });
      },
    );
    Widget continueButton = FlatButton(
      child: Text("بله"),
      onPressed: () {
        setState(() {
          Navigator.of(context).pop(); // dismiss dialog
          courseStore.deleteCourseFromBasket(course);
        });
      },
    );

    // set up the AlertDialog
    AlertDialog alert = AlertDialog(
      title: Text("هشدار"),
      content: Text("آیا از حذف دوره از سبد خرید مطمئنید؟"),
      actions: [
        cancelButton,
        continueButton,
      ],
    );

    // show the dialog
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return alert;
      },
    );
  }

  Future loginStatement() async{
    String token = await secureStorage.read(key: 'token');
    if(token.isNotEmpty && !courseStore.isTokenExpired(token))
      courseStore.setUserDetails(token);
  }

  @override
  Widget build(BuildContext context) {
    courseStore = Provider.of<CourseStore>(context);
    courseStore.setAllCourses(courseList);


    // FirebaseAdMob.instance
    //     .initialize(appId: "ca-app-pub-6716792328957551~1144830596")
    //     .then((value) => myBanner
    //       ..load()
    //       ..show(anchorType: AnchorType.bottom));

    width = MediaQuery.of(context).size.width / 2;
    height = (MediaQuery.of(context).size.width / 2) * 1.5;
    return FutureBuilder(
        future: courses,
        builder: (context, data) {
          if (data.hasData)
            return WillPopScope(
                child: Scaffold(
                    bottomNavigationBar: Padding(
                      padding: const EdgeInsets.only(bottom: 0),
                      child: CurvedNavigationBar(
                        color: Color(0xFF202028),
                        buttonBackgroundColor: Color(0xFF202028),
                        animationDuration: Duration(milliseconds: 200),
                        height: 50,
                        backgroundColor: Color(0xFF34333A),
                        items: <Widget>[
                          Icon(Icons.person,
                              size: 25, color: Color(0xFF20BFA9)),
                          Icon(Icons.home, size: 25, color: Color(0xFF20BFA9)),
                          Icon(Icons.shopping_basket,
                              size: 25, color: Color(0xFF20BFA9)),
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
              color: Color(0xFF202028),
              child: SpinKitWave(
                color: Color(0xFF20BFA9),
                size: 100.0,
              ),
            );
        });
  }
}
//
// MobileAdTargetingInfo targetingInfo = MobileAdTargetingInfo(
//   keywords: <String>['podcast', 'hadi'],
//   contentUrl: 'https://flutter.io',
//   childDirected: false,
//   testDevices: <String>[
//     'A36235BD5DAEAA4D6FA305A209159D2A'
//   ], // Android emulators are considered test devices
// );
//
// BannerAd myBanner = BannerAd(
//   // Replace the testAdUnitId with an ad unit id from the AdMob dash.
//   // https://developers.google.com/admob/android/test-ads
//   // https://developers.google.com/admob/ios/test-ads
//   adUnitId: BannerAd.testAdUnitId,
//   size: AdSize.fullBanner,
//   targetingInfo: targetingInfo,
//   listener: (MobileAdEvent event) {
//     print("BannerAd event is $event");
//   },
// );
//
// InterstitialAd myInterstitial = InterstitialAd(
//   // Replace the testAdUnitId with an ad unit id from the AdMob dash.
//   // https://developers.google.com/admob/android/test-ads
//   // https://developers.google.com/admob/ios/test-ads
//   adUnitId: InterstitialAd.testAdUnitId,
//   targetingInfo: targetingInfo,
//   listener: (MobileAdEvent event) {
//     print("InterstitialAd event is $event");
//   },
// );
