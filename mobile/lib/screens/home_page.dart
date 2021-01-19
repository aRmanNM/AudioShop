import 'package:curved_navigation_bar/curved_navigation_bar.dart';
import 'package:flutter/material.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:flutter_cache_manager/flutter_cache_manager.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:intl/intl.dart';
import 'package:mobile/models/course.dart';
import 'package:mobile/screens/checkout_page.dart';
import 'package:mobile/screens/authentication_page.dart';
import 'package:mobile/services/course_service.dart';
import 'package:mobile/shared/enums.dart';
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
  Widget dropdownValue = Icon(Icons.person_pin, size: 50, color: Colors.white,);
  bool alertReturn = false;

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

  goToCoursePage(Course course, var courseCover) {
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
        Card(
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(8.0),
          ),
          color: Color(0xFF2c3335),
          child: TextButton(
            style: ButtonStyle(
              padding: MaterialStateProperty.all(
                  EdgeInsets.symmetric(vertical: 0, horizontal: 0)),
            ),
            onPressed: () {
              goToCoursePage(course, pictureFile);
            },
            child: Column(
              children: <Widget>[
                ClipRRect(
                  borderRadius: BorderRadius.vertical(top: Radius.circular(8)),
                  child: Image.file(
                    pictureFile,
                    fit: BoxFit.fill,
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.all(3.0),
                  child: Text(
                    courseName,
                    overflow: TextOverflow.ellipsis,
                    style: TextStyle(fontSize: 14, color: Colors.white),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.all(2.0),
                  child: Text(
                    'بیطرف',
                    overflow: TextOverflow.ellipsis,
                    style: TextStyle(fontSize: 12, color: Colors.white70),
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
        Expanded(
          flex: 2,
          child: Card(
            color: Color(0xFF403F44),
            child: SafeArea (
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
        ),
        Expanded(
          flex: 3,
          child: Card(
            color: Color(0xFF403F44),
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
                child: SafeArea (
                  child: ListView.builder(
                      itemCount: courseStore.basket.length,
                      itemBuilder: (BuildContext context, int index) {
                        return Padding(
                          padding: const EdgeInsets.symmetric(
                              vertical: 2, horizontal: 8),
                          child: Card(
                            color: Color(0xFF403F44),
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
                                          onPressed: () async {
                                            Widget cancelB = cancelButton('خیر');
                                            Widget continueB =
                                              continueButton('بله', Alert.DeleteFromBasket, index);
                                            AlertDialog alertD = alert('هشدار',
                                                'آیا از حذف دوره از سبد خرید مطمئنید؟',
                                                [cancelB, continueB]);

                                            showBasketAlertDialog(context, alertD);

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
                                  padding:
                                      const EdgeInsets.fromLTRB(8, 8, 8, 8),
                                  child: Text(
                                    basketPrice(),
                                  ),
                                )),
                          ],
                        ),
                      ),
                    ),
                    Expanded(
                      flex: 3,
                      child: Padding(
                        padding: const EdgeInsets.fromLTRB(15, 0, 15, 20),
                        child: Card(
                          color: Color(0xFF20BFA9),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(15.0),
                          ),
                          child: TextButton(
                            onPressed: () {
                              if (courseStore.token != null && courseStore.token != '')
                                Navigator.push(context,
                                    MaterialPageRoute(builder: (context) {
                                  return CheckOutPage();
                                }));
                              else {
                                Navigator.push(context,
                                    MaterialPageRoute(builder: (context) {
                                  return AuthenticationPage(FormName.SignUp);
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

    courseStore.setTotalBasketPrice(totalBasketPrice.toInt());

    return NumberFormat('#,###').format(totalBasketPrice) + ' تومان';
  }

  Widget library() {
    return (courseStore.token == null || courseStore.token == '')
        ? Column(
            children: <Widget>[
              Expanded(
                flex: 3,
                child: SafeArea (
                  child: Text('جهت استفاده از محتوای غیر رایگان برنامه لطفا'
                      ' ثبت نام کنید یا اگر قبلا ثبت نامه کرده اید وارد شوید.'),
                ),
              ),
              Expanded(
                  flex: 1,
                  child: Row(
                    children: <Widget>[
                      Expanded(
                        child: TextButton(
                          child: Text('ورود'),
                          onPressed: () {
                            Navigator.push(context,
                                MaterialPageRoute(builder: (context) {
                              return AuthenticationPage(FormName.SignIn);
                            }));
                          },
                        ),
                      ),
                      Expanded(
                        child: TextButton(
                          child: Text('ثبت نام'),
                          onPressed: () {
                            Navigator.push(context,
                                MaterialPageRoute(builder: (context) {
                              return AuthenticationPage(FormName.SignUp);
                            }));
                          },
                        ),
                      ),
                    ],
                  ))
            ],
          )
        : SafeArea(
            child: Column(
              children: <Widget>[
                Expanded(
                  flex: 3,
                  child: Card(
                    color: Color(0xFF202028),
                    elevation: 20,
                    child: Row(
                      children: <Widget>[
                        Expanded(
                            flex: 1,
                            child: Icon(
                              Icons.person_pin,
                              size: 50,
                              color: Colors.white,
                            ),
                        ),
                        Expanded(
                            flex: courseStore.hasPhoneNumber ? 4 : 3,
                            child: Padding(
                              padding: const EdgeInsets.fromLTRB(8,0,8,0),
                              child: Text(courseStore.userName),
                            ),
                        ),
                        registerPhoneButton(),
                        Expanded(
                          flex: 2,
                          child: TextButton(
                            onPressed: () async {
                              Widget cancelB = cancelButton('خیر');
                              Widget continueB =
                                continueButton('بله', Alert.LogOut, null);
                              AlertDialog alertD = alert('هشدار',
                                  'میخواهید از برنامه خارج شوید؟',
                                  [cancelB, continueB]);

                              await showBasketAlertDialog(context, alertD);

                              if(alertReturn){
                                await logOut();
                              }
                              alertReturn = false;

                              setState(() {
                                navigationSelect(1);
                              });
                            },
                            child: Card(
                              color: Colors.red[700],
                              child: Center(child: Text('خروج')),
                            )
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
                notRegisteredPhoneNumber(),
                Expanded(
                  flex: 1,
                  child: Padding(
                    padding: const EdgeInsets.only(right: 8.0),
                    child: Center(
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.start,
                          children: [
                            Text(courseStore.userCourses.length > 0 ?
                              'دوره های شما' : 'هنوز دوره ای در حساب کاربری شما ثبت نشده است',
                              style: TextStyle(fontSize: 18),
                            ),
                          ],
                        ),
                    ),
                  ),
                ),
                Expanded(
                  flex: 15,
                  child: userCourses(),
                )
              ],
            ),
        );
  }

  Widget registerPhoneButton(){
    if(courseStore.hasPhoneNumber)
      return SizedBox();
    return Expanded(
      flex: 2,
      child: TextButton(
        onPressed: (){
          Navigator.push(context, MaterialPageRoute(builder: (context){
            return AuthenticationPage(FormName.RegisterPhoneNumber);
          }));
        },
        child: Card(
          color: Color(0xFF20BFA9),
          child: Center(child: Text('ثبت همراه')),
        ),
      ),
    );
  }

  Widget notRegisteredPhoneNumber(){
    if(courseStore.token != null &&
        courseStore.token != '' &&
        !courseStore.hasPhoneNumber){
      return Expanded(
        flex: 4,
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Text(
            'کاربر عزیز. شماره همراه شما در سیستم ثبت نشده است.'
                ' ورود مجدد به حساب کاربری فقط با شماره همراه ممکن است.'
                ' آیا مایل به ثبت شماره همراه خود هستید؟',
            style: TextStyle(color: Colors.red[300]),
      ),
        ));
    }
    return SizedBox();
  }

  Widget userCourses(){
    return ListView.builder(
        itemCount: courseStore.userCourses.length,
        itemBuilder: (BuildContext context, int index) {
          return TextButton(
            onPressed: () async {
              var picFile = await DefaultCacheManager().getSingleFile(
                  courseStore.userCourses[index].pictureUrl);
              goToCoursePage(courseStore.userCourses[index], picFile);
            },
            child: Card(
              color: Color(0xFF403F44),
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
                              courseStore.userCourses[index].pictureUrl),
                        )),
                    Expanded(
                      flex: 6,
                      child: Center(
                        child: Padding(
                          padding: const EdgeInsets.fromLTRB(8,0,8,0),
                          child: Text(
                            courseStore.userCourses[index].name,
                            style: TextStyle(fontSize: 19),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          );
        });
  }

  Future logOut() async{
    await secureStorage.write(key: 'token', value: '');
    await courseStore.setUserDetails('', false);
  }

  Widget cancelButton(String cancelText){
    return FlatButton(
      child: Text(cancelText),
      onPressed: () {
        Navigator.of(context).pop();
        alertReturn = false;
      },
    );
  }

  Widget continueButton(String continueText, Alert alert, int index){
    return FlatButton(
      child: Text(continueText),
      onPressed: () async {
        Navigator.of(context).pop();
        if(alert == Alert.DeleteFromBasket)
          courseStore.deleteCourseFromBasket(courseStore.basket[index]);
        else if(alert == Alert.LogOut){
          alertReturn = true;
        }
        else if(alert == Alert.RegisterPhoneNumber){
          Navigator.push(context, MaterialPageRoute(builder: (context){
            return AuthenticationPage(FormName.RegisterPhoneNumber);
          }));
        }
      },
    );
  }

  AlertDialog alert(String titleText, String contentText, List<Widget> actions){
    return AlertDialog(
      title: Text(titleText),
      content: Text(contentText),
      actions: actions,
    );
  }

  Future showBasketAlertDialog(BuildContext context, AlertDialog alert) async {
    return await showDialog(
      context: context,
      builder: (BuildContext context) {
        return alert;
      },
    );
  }

  Future loginStatement() async {
    String token = await secureStorage.read(key: 'token');
    String hasPhoneNumber = await secureStorage.read(key: 'hasPhoneNumber');
    if (token.isNotEmpty && !courseStore.isTokenExpired(token))
      await courseStore.setUserDetails(token, hasPhoneNumber.toLowerCase() == 'true');
  }

  @override
  Widget build(BuildContext context) {
    courseStore = Provider.of<CourseStore>(context);
    courseStore.setAllCourses(courseList);
    if(courseStore.token != null)
      courseStore.setUserDetails(courseStore.token, courseStore.hasPhoneNumber);
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
