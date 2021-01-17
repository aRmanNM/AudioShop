import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:mobile/models/course.dart';
import 'package:mobile/models/user.dart';
import 'package:mobile/shared/enums.dart';
import 'package:mobile/store/course_store.dart';
import 'package:async/async.dart';
import 'package:mobile/services/authentication_service.dart';
import 'package:provider/provider.dart';


class AuthenticationPage extends StatefulWidget {
  AuthenticationPage(this.baseForm);

  final baseForm;
  @override
  _AuthenticationPageState createState() => _AuthenticationPageState();
}

class _AuthenticationPageState extends State<AuthenticationPage> {
  var formName = FormName.SignUp;
  AuthenticationService authService = AuthenticationService();
  TextEditingController phoneNumberController = new TextEditingController();
  TextEditingController nameController = new TextEditingController();
  TextEditingController userNameController = new TextEditingController();
  TextEditingController passwordController = new TextEditingController();
  TextEditingController confirmPasswordController = new TextEditingController();
  TextEditingController verificationCodeController =
      new TextEditingController();
  TextEditingController presenterController = new TextEditingController();
  final secureStorage = FlutterSecureStorage();
  CourseStore courseStore;
  Duration _timerDuration = new Duration(seconds: 60);
  RestartableTimer _timer;
  bool sentCode = false;
  bool isTimerActive = false;
  bool isCheckingUserName = false;
  String phoneNumberError = '';
  String verificationCodeError = '';
  String userNameError = '';
  String passwordError = '';

  @override
  void initState() {
    super.initState();
    formName = widget.baseForm;
  }

  Future receiveCode() async {
    _timer = RestartableTimer(_timerDuration, setTimerState);

    setState(() {
      isTimerActive = true;
    });

    bool isRepetitiveUser = await authService
        .isPhoneNumberRegistered(phoneNumberController.text);

    if (formName == FormName.SignIn) {
      if (isRepetitiveUser) {
        sentCode = await authService.sendVerificationCode(
            'https://audioshoppp.ir/api/auth/login',
            phoneNumberController.text);
      } else {
        Fluttertoast.showToast(
            msg: 'کاربری با این شماره تلفن یافت نشد. لطفا ثبت نام کنید.');
      }
    } else {
      if(!isRepetitiveUser){
        sentCode = await authService.
          verifyPhoneNumber(phoneNumberController.text, courseStore.userId);
      }
      else {
        Fluttertoast.showToast(msg: 'شماره همراه تکراری است. کافی است وارد شوید.');
        return;
      }
    }

    if (sentCode)
      Fluttertoast.showToast(msg: 'کد تایید برای شما ارسال شد');
    else{
      Fluttertoast.showToast(msg: 'کد تایید ارسال نشد. لطفا مجددا امتحان کنید');
      setState(() {
        isTimerActive = false;
      });
    }

    sentCode = false;
  }

  Future<bool> isUserNameRepetitive(String username) async {
    var usernameExists = await authService.usernameExists(username);
    if (usernameExists)
      Fluttertoast.showToast(
          msg: 'نام کاربری تکراری است. لطفا آن را تغییر دهید');
    else if (!usernameExists)
      Fluttertoast.showToast(msg: 'نام کاربری در دسترس است');
    else
      Fluttertoast.showToast(
          msg: 'مشکل در برقراری ارتباط. لطفا مجددا تلاش کنید');
    return usernameExists;
  }

  Widget sendCodeButton() {
    return Card(
      color: (!isTimerActive) ? Colors.red[700] : Colors.red[200],
      child: TextButton(
        onPressed: () {
          setState(() {
            if (!isTimerActive) {
              receiveCode();
            }
          });
        },
        child: Text(
          (!isTimerActive) ? 'دریافت کد' : 'کد ارسال شد',
          style: TextStyle(
            fontSize: 16,
            fontWeight: FontWeight.bold,
            color: Colors.white,
          ),
        ),
      ),
    );
  }

  void setTimerState() {
    setState(() {
      isTimerActive = false;
    });
  }

  Future registerPhoneNumber() async{
    bool registeredPhone = await authService.registerPhoneNumber(
        phoneNumberController.text, verificationCodeController.text, courseStore.userId);
    if (!registeredPhone)
      Fluttertoast.showToast(
          msg: 'ثبت شماره با مشکل مواجه شد. لطفا مجددا تلاش کنید.');
    else {
      await secureStorage.write(
          key: 'hasPhoneNumber', value: true.toString());

      await courseStore.setUserDetails(courseStore.token, true);

      Navigator.pop(context);
    }
  }

  Future signUp() async {
    bool isUserNotOk = await isUserNameRepetitive(userNameController.text);
    if (!isUserNotOk) {
      User registeredUser = await authService.signUp(
          userNameController.text, passwordController.text);
      if (registeredUser == null)
        Fluttertoast.showToast(
            msg: 'ثبت نام با مشکل مواجه شد. لطفا مجددا تلاش کنید.');
      else {
        await secureStorage.write(key: 'token', value: registeredUser.token);
        await secureStorage.write(
            key: 'hasPhoneNumber',
            value: registeredUser.hasPhoneNumber.toString());

        await courseStore.setUserDetails(registeredUser.token, registeredUser.hasPhoneNumber);

        List<Course> userCourses = await authService.getUserCourses(
            courseStore.userId, courseStore.token);

        List<Course> tempBasket = List.from(courseStore.basket);

        for (Course basketItem in courseStore.basket) {
          for (Course course in userCourses) {
            if (basketItem.id == course.id) {
              tempBasket.remove(basketItem);
            }
          }
        }

        courseStore.refineUserBasket(tempBasket);

        Navigator.pop(context);
      }
    }
  }

  Future signIn() async {
    User loggedInUser = await authService.signIn(
        phoneNumberController.text, verificationCodeController.text);
    if (loggedInUser == null)
      Fluttertoast.showToast(
          msg: 'ثبت نام با مشکل مواجه شد. لطفا مجددا تلاش کنید.');
    else {
      await secureStorage.write(key: 'token', value: loggedInUser.token);
      await secureStorage.write(
          key: 'hasPhoneNumber', value: loggedInUser.hasPhoneNumber.toString());

      await courseStore.setUserDetails(loggedInUser.token, loggedInUser.hasPhoneNumber);

      List<Course> userCourses = await authService.getUserCourses(
          courseStore.userId, courseStore.token);

      List<Course> tempBasket = List.from(courseStore.basket);

      for (Course basketItem in courseStore.basket) {
        for (Course course in userCourses) {
          if (basketItem.id == course.id) {
            tempBasket.remove(basketItem);
          }
        }
      }

      courseStore.refineUserBasket(tempBasket);

      Navigator.pop(context);
    }
  }

  Widget authForm(FormName formName) {
    if (formName == FormName.SignIn)
      return SafeArea(
        child: SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.fromLTRB(45, 0, 45, 0),
            child: Center(
              child: IntrinsicWidth(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: <Widget>[
                    Center(
                      child: Text(
                        'برای ورود به حساب کاربری، شماره همراه خود را وارد کنید',
                        style:
                            TextStyle(fontSize: 25, fontWeight: FontWeight.bold),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(top: 28.0),
                      child: TextField(
                        style: TextStyle(
                            decorationColor: Colors.black, color: Colors.white),
                        keyboardType: TextInputType.phone,
                        decoration: InputDecoration(
                          enabledBorder: OutlineInputBorder(
                            borderSide:
                                BorderSide(color: Colors.white, width: 2.0),
                          ),
                          focusedBorder: OutlineInputBorder(
                            borderSide:
                                BorderSide(color: Colors.white, width: 2.0),
                          ),
                          border: OutlineInputBorder(),
                          labelStyle: TextStyle(
                            color: Colors.white,
                          ),
                          labelText: 'شماره همراه',
                        ),
                        controller: phoneNumberController,
                      ),
                    ),
                    SizedBox(
                      height: 30,
                      child: Text(
                        phoneNumberError,
                        style: TextStyle(color: Colors.red[200]),
                      ),
                    ),
                    IntrinsicHeight(
                      child: Row(
                        crossAxisAlignment: CrossAxisAlignment.stretch,
                        children: <Widget>[
                          Expanded(
                            flex: 1,
                            child: sendCodeButton(),
                          ),
                          Expanded(
                            flex: 2,
                            child: TextField(
                              style: TextStyle(color: Colors.white),
                              keyboardType: TextInputType.phone,
                              decoration: InputDecoration(
                                border: OutlineInputBorder(),
                                enabledBorder: OutlineInputBorder(
                                  borderSide:
                                      BorderSide(color: Colors.white, width: 2.0),
                                ),
                                focusedBorder: OutlineInputBorder(
                                  borderSide:
                                      BorderSide(color: Colors.white, width: 2.0),
                                ),
                                labelText: 'کد دریافتی',
                                labelStyle: TextStyle(
                                  color: Colors.white,
                                ),
                              ),
                              controller: verificationCodeController,
                            ),
                          ),
                        ],
                      ),
                    ),
                    SizedBox(
                      height: 20,
                      child: Text(
                        verificationCodeError,
                        style: TextStyle(color: Colors.red[200]),
                      ),
                    ),
                    Card(
                      color: Color(0xFF20BFA9),
                      child: TextButton(
                        onPressed: () async {
                          setState(() {
                            phoneNumberError = verificationCodeError = '';
                            if (phoneNumberController.text.isEmpty)
                              phoneNumberError = 'شماره موبایل الزامی است';
                            if (verificationCodeController.text.isEmpty)
                              verificationCodeError =
                                  'کد ارسال شده به همراهتان را وارد کنید';
                          });
                          if (phoneNumberController.text.isNotEmpty &&
                              verificationCodeController.text.isNotEmpty)
                            await signIn();
                          //TODO SignIn Method
                        },
                        child: Text(
                          'تایید',
                          style: TextStyle(
                            fontSize: 25,
                            fontWeight: FontWeight.bold,
                            color: Colors.white,
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      );
    else if (formName == FormName.RegisterPhoneNumber)
      return SafeArea(
        child: SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.fromLTRB(45, 0, 45, 0),
            child: Center(
              child: IntrinsicWidth(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: <Widget>[
                    Center(
                      child: Text(
                        'لطفا شماره همراه خود را جهت بازیابی وارد کنید',
                        style:
                            TextStyle(fontSize: 25, fontWeight: FontWeight.bold),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(top: 28.0),
                      child: TextField(
                        style: TextStyle(
                            decorationColor: Colors.black, color: Colors.white),
                        keyboardType: TextInputType.phone,
                        decoration: InputDecoration(
                          enabledBorder: OutlineInputBorder(
                            borderSide:
                                BorderSide(color: Colors.white, width: 2.0),
                          ),
                          focusedBorder: OutlineInputBorder(
                            borderSide:
                                BorderSide(color: Colors.white, width: 2.0),
                          ),
                          border: OutlineInputBorder(),
                          labelStyle: TextStyle(
                            color: Colors.white,
                          ),
                          labelText: 'شماره همراه',
                        ),
                        controller: phoneNumberController,
                      ),
                    ),
                    SizedBox(
                      height: 20,
                      child: Text(
                        phoneNumberError,
                        style: TextStyle(color: Colors.red[200]),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(top: 28.0),
                      child: IntrinsicHeight(
                        child: Row(
                          crossAxisAlignment: CrossAxisAlignment.stretch,
                          children: <Widget>[
                            Expanded(
                              flex: 1,
                              child: sendCodeButton(),
                            ),
                            Expanded(
                              flex: 2,
                              child: TextField(
                                style: TextStyle(color: Colors.white),
                                keyboardType: TextInputType.phone,
                                decoration: InputDecoration(
                                  border: OutlineInputBorder(),
                                  enabledBorder: OutlineInputBorder(
                                    borderSide: BorderSide(
                                        color: Colors.white, width: 2.0),
                                  ),
                                  focusedBorder: OutlineInputBorder(
                                    borderSide: BorderSide(
                                        color: Colors.white, width: 2.0),
                                  ),
                                  labelText: 'کد دریافتی',
                                  labelStyle: TextStyle(
                                    color: Colors.white,
                                  ),
                                ),
                                controller: verificationCodeController,
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                    SizedBox(
                      height: 20,
                      child: Text(
                        verificationCodeError,
                        style: TextStyle(color: Colors.red[200]),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(top: 28.0),
                      child: Card(
                        color: Color(0xFF20BFA9),
                        child: TextButton(
                          onPressed: () async {
                            setState(() {
                              phoneNumberError = verificationCodeError = '';
                              if (phoneNumberController.text.isEmpty)
                                phoneNumberError = 'شماره موبایل الزامی است';
                              if (verificationCodeController.text.isEmpty)
                                verificationCodeError =
                                    'کد ارسال شده به همراهتان را وارد کنید';

                            });
                            if(phoneNumberController.text.isNotEmpty &&
                              verificationCodeController.text.isNotEmpty)
                              await registerPhoneNumber();
                          },
                          child: Text(
                            'تایید',
                            style: TextStyle(
                              fontSize: 25,
                              fontWeight: FontWeight.bold,
                              color: Colors.white,
                            ),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      );
    else
      return SafeArea(
        child: SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.fromLTRB(45, 0, 45, 0),
            child: Center(
              child: IntrinsicWidth(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: <Widget>[
                    Center(
                      child: Text(
                        'جهت ثبت نام موارد زیر را کامل کنید',
                        style:
                            TextStyle(fontSize: 25, fontWeight: FontWeight.bold),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(top: 28.0),
                      child: IntrinsicHeight(
                        child: Row(
                          crossAxisAlignment: CrossAxisAlignment.stretch,
                          children: [
                            Expanded(
                              flex: 5,
                              child: TextField(
                                style: TextStyle(
                                    decorationColor: Colors.black,
                                    color: Colors.white),
                                keyboardType: TextInputType.text,
                                decoration: InputDecoration(
                                  enabledBorder: OutlineInputBorder(
                                    borderSide: BorderSide(
                                        color: Colors.white, width: 2.0),
                                  ),
                                  focusedBorder: OutlineInputBorder(
                                    borderSide: BorderSide(
                                        color: Colors.white, width: 2.0),
                                  ),
                                  border: OutlineInputBorder(),
                                  labelStyle: TextStyle(
                                    color: Colors.white,
                                  ),
                                  labelText: 'نام کاربری',
                                ),
                                controller: userNameController,
                              ),
                            ),
                            Expanded(
                              flex: 2,
                              child: Card(
                                color: !isCheckingUserName
                                    ? Colors.red[700]
                                    : Colors.red[200],
                                child: TextButton(
                                  onPressed: () {
                                    setState(() {
                                      if (!isCheckingUserName) {
                                        isCheckingUserName = true;
                                        isUserNameRepetitive(
                                            userNameController.text);
                                        isCheckingUserName = false;
                                      }
                                    });
                                  },
                                  child: Text(
                                    (!isCheckingUserName)
                                        ? 'بررسی کن'
                                        : 'در حال بررسی',
                                    style: TextStyle(
                                      fontSize: 16,
                                      fontWeight: FontWeight.bold,
                                      color: Colors.white,
                                    ),
                                  ),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                    SizedBox(
                      height: 25,
                      child: Text(
                        userNameError,
                        style: TextStyle(color: Colors.red[200]),
                      ),
                    ),
                    IntrinsicHeight(
                      child: Row(
                        crossAxisAlignment: CrossAxisAlignment.stretch,
                        children: <Widget>[
                          Expanded(
                            child: Padding(
                              padding: const EdgeInsets.only(left: 4.0),
                              child: TextField(
                                style: TextStyle(color: Colors.white),
                                keyboardType: TextInputType.text,
                                decoration: InputDecoration(
                                  border: OutlineInputBorder(),
                                  enabledBorder: OutlineInputBorder(
                                    borderSide: BorderSide(
                                        color: Colors.white, width: 2.0),
                                  ),
                                  focusedBorder: OutlineInputBorder(
                                    borderSide: BorderSide(
                                        color: Colors.white, width: 2.0),
                                  ),
                                  labelText: 'رمز عبور',
                                  labelStyle: TextStyle(
                                    color: Colors.white,
                                  ),
                                ),
                                controller: passwordController,
                              ),
                            ),
                          ),
                          Expanded(
                            child: Padding(
                              padding: const EdgeInsets.only(right: 4.0),
                              child: TextField(
                                style: TextStyle(color: Colors.white),
                                keyboardType: TextInputType.text,
                                decoration: InputDecoration(
                                  border: OutlineInputBorder(),
                                  enabledBorder: OutlineInputBorder(
                                    borderSide: BorderSide(
                                        color: Colors.white, width: 2.0),
                                  ),
                                  focusedBorder: OutlineInputBorder(
                                    borderSide: BorderSide(
                                        color: Colors.white, width: 2.0),
                                  ),
                                  labelText: 'تکرار رمز عبور',
                                  labelStyle: TextStyle(
                                    color: Colors.white,
                                  ),
                                ),
                                controller: confirmPasswordController,
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                    SizedBox(
                      height: 20,
                      child: Text(
                        passwordError,
                        style: TextStyle(color: Colors.red[200]),
                      ),
                    ),
                    Card(
                      color: Color(0xFF20BFA9),
                      child: TextButton(
                        onPressed: () async {
                          setState(() {
                            userNameError = passwordError = '';
                            if (userNameController.text.isEmpty)
                              userNameError = 'نام کاربری الزامی است';
                            if (passwordController.text.isEmpty)
                              passwordError = 'رمز عبور الزامی است';
                            else if (confirmPasswordController.text.isEmpty ||
                                passwordController.text !=
                                    confirmPasswordController.text)
                              passwordError = 'رمز عبور مطابقت ندارد';
                          });
                          if (userNameController.text.isNotEmpty &&
                              passwordController.text.isNotEmpty &&
                              confirmPasswordController.text.isNotEmpty)
                            await signUp();
                        },
                        child: Text(
                          'تایید',
                          style: TextStyle(
                            fontSize: 25,
                            fontWeight: FontWeight.bold,
                            color: Colors.white,
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      );
  }

  @override
  Widget build(BuildContext context) {
    courseStore = Provider.of<CourseStore>(context);

    return Scaffold(
      body: authForm(formName),
      persistentFooterButtons: <Widget>[
        SizedBox(
          width: MediaQuery.of(context).size.width / 2 - 12,
          child: Card(
            color: formName == FormName.SignIn
                ? Color(0xFF20BFA9)
                : Color(0xFF202028),
            child: TextButton(
              onPressed: () {
                setState(() {
                  formName = FormName.SignIn;
                });
              },
              child: Text(
                'ورود',
                style: TextStyle(
                  fontSize: 19,
                  fontWeight: FontWeight.bold,
                  color: Colors.white,
                ),
              ),
            ),
          ),
        ),
        SizedBox(
          width: MediaQuery.of(context).size.width / 2 - 12,
          child: Card(
            color: (formName == FormName.SignUp ||
                    formName == FormName.RegisterPhoneNumber)
                ? Color(0xFF20BFA9)
                : Color(0xFF202028),
            child: TextButton(
              onPressed: () {
                setState(() {
                  formName = FormName.SignUp;
                });
              },
              child: Text(
                formName == FormName.RegisterPhoneNumber
                    ? 'ثبت شماره همراه'
                    : 'ثبت نام',
                style: TextStyle(
                  fontSize: 19,
                  fontWeight: FontWeight.bold,
                  color: Colors.white,
                ),
              ),
            ),
          ),
        ),
      ],
    );
  }
}
