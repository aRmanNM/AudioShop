import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:mobile/store/course_store.dart';
import 'package:async/async.dart';
import 'package:mobile/services/authentication_service.dart';

enum FormName{
  SignIn,
  SignUp,
  RegisterPhoneNumber,
}

class AuthenticationPage extends StatefulWidget {

  AuthenticationPage();

  @override
  _AuthenticationPageState createState() => _AuthenticationPageState();
}

class _AuthenticationPageState extends State<AuthenticationPage> {
  var formName = FormName.SignUp;
  TextEditingController phoneNumberController = new TextEditingController();
  TextEditingController nameController = new TextEditingController();
  TextEditingController userNameController = new TextEditingController();
  TextEditingController passwordController = new TextEditingController();
  TextEditingController confirmPasswordController = new TextEditingController();
  TextEditingController verificationCodeController = new TextEditingController();
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
    //_timer = new RestartableTimer(Duration(seconds: 0), null);
  }

  Future receiveCode() async{

    _timer = RestartableTimer(_timerDuration, setTimerState);

    setState(() {
      isTimerActive = true;
    });

    AuthenticationService authService = AuthenticationService();
    bool isRepetitiveUser = await authService.isPhoneNumberRegistered(
        'https://audioshoppp.ir/api/auth/phoneexists?phoneNumber='
        + phoneNumberController.text);
    {
      if(formName == FormName.SignIn){
        if(isRepetitiveUser){
          sentCode = await authService.
          signIn('https://audioshoppp.ir/api/auth/login',
              phoneNumberController.text);
        }
        else{
          Fluttertoast.showToast(msg: 'کاربری با این شماره تلفن یافت نشد. لطفا ثبت نام کنید.');
        }
      }
      else{
        if(!isRepetitiveUser){
          sentCode = await authService.
          signUp('https://audioshoppp.ir/api/auth/register',
              phoneNumberController.text, nameController.text);
        }
        else {
          Fluttertoast.showToast(msg: 'شماره همراه تکراری است. کافی است وارد شوید.');
        }
      }
    }

    if(sentCode)
      Fluttertoast.showToast(msg: 'کد تایید برای شما ارسال شد');
    else
      Fluttertoast.showToast(msg: 'کد تایید ارسال نشد. لطفا مجددا امتحان کنید');

    sentCode = false;
  }
  
  Future<bool> isUserNameRepetitive(String userName) async{
    return true;
  }

  Widget sendCodeButton(){
    return Card(
      color: (!isTimerActive) ?
      Colors.red[700] : Colors.red[400],
      child: TextButton(
        onPressed: (){
          setState(() async {
            if(!isTimerActive){
              await receiveCode();
            }
          });
        },
        child: Text((!isTimerActive) ?
        'دریافت کد' : 'کد ارسال شد',
          style: TextStyle(
            fontSize: 16,
            fontWeight: FontWeight.bold,
            color: Colors.white,
          ),
        ),
      ),
    );
  }

  void setTimerState(){
    setState(() {
      isTimerActive = false;
    });
  }


  Widget authForm(FormName formName){
    if(formName == FormName.SignIn)
      return SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(45,60,45,0),
          child: Center(
            child: IntrinsicWidth(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  Center(
                    child: Text(
                      'برای ورود به حساب کاربری، شماره همراه خود را وارد کنید',
                      style: TextStyle(
                        fontSize: 25,
                        fontWeight: FontWeight.bold
                      ),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 28.0),
                    child: TextField(
                      style: TextStyle(
                        decorationColor: Colors.black,
                        color: Colors.white
                      ),
                      keyboardType: TextInputType.phone,
                      decoration: InputDecoration(
                        enabledBorder: OutlineInputBorder(
                          borderSide: BorderSide(color: Colors.white, width: 2.0),
                        ),
                        focusedBorder: OutlineInputBorder(
                          borderSide: BorderSide(color: Colors.white, width: 2.0),
                        ),
                        border: OutlineInputBorder(),
                        labelStyle: TextStyle(color: Colors.white,),
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
                            style: TextStyle(
                              color: Colors.white
                            ),
                            keyboardType: TextInputType.phone,
                            decoration: InputDecoration(
                              border: OutlineInputBorder(),
                              enabledBorder: OutlineInputBorder(
                                borderSide: BorderSide(color: Colors.white, width: 2.0),
                              ),
                              focusedBorder: OutlineInputBorder(
                                borderSide: BorderSide(color: Colors.white, width: 2.0),
                              ),
                              labelText: 'کد دریافتی',
                              labelStyle: TextStyle(color: Colors.white,),
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
                      onPressed: (){
                        setState(() {
                          phoneNumberError = verificationCodeError = '';
                          if(phoneNumberController.text.isEmpty)
                            phoneNumberError = 'شماره موبایل الزامی است';
                          if(verificationCodeController.text.isEmpty)
                            verificationCodeError = 'کد ارسال شده به همراهتان را وارد کنید';
                          // if(phoneNumberController.text.isNotEmpty &&
                          // verificationCodeController.text.isNotEmpty)
                          //TODO SignIn Method
                        });
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
      );
    else if(formName == FormName.RegisterPhoneNumber)
      return SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(45,60,45,0),
          child: Center(
            child: IntrinsicWidth(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  Center(
                    child: Text(
                      'لطفا شماره همراه خود را جهت بازیابی وارد کنید',
                      style: TextStyle(
                          fontSize: 25,
                          fontWeight: FontWeight.bold
                      ),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 28.0),
                    child: TextField(
                      style: TextStyle(
                          decorationColor: Colors.black,
                          color: Colors.white
                      ),
                      keyboardType: TextInputType.phone,
                      decoration: InputDecoration(
                        enabledBorder: OutlineInputBorder(
                          borderSide: BorderSide(color: Colors.white, width: 2.0),
                        ),
                        focusedBorder: OutlineInputBorder(
                          borderSide: BorderSide(color: Colors.white, width: 2.0),
                        ),
                        border: OutlineInputBorder(),
                        labelStyle: TextStyle(color: Colors.white,),
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
                              style: TextStyle(
                                  color: Colors.white
                              ),
                              keyboardType: TextInputType.phone,
                              decoration: InputDecoration(
                                border: OutlineInputBorder(),
                                enabledBorder: OutlineInputBorder(
                                  borderSide: BorderSide(color: Colors.white, width: 2.0),
                                ),
                                focusedBorder: OutlineInputBorder(
                                  borderSide: BorderSide(color: Colors.white, width: 2.0),
                                ),
                                labelText: 'کد دریافتی',
                                labelStyle: TextStyle(color: Colors.white,),
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
                        onPressed: (){
                          setState(() {
                            phoneNumberError = verificationCodeError = '';
                            if(phoneNumberController.text.isEmpty)
                              phoneNumberError = 'شماره موبایل الزامی است';
                            if(verificationCodeController.text.isEmpty)
                              verificationCodeError = 'کد ارسال شده به همراهتان را وارد کنید';
                            // if(phoneNumberController.text.isNotEmpty &&
                            // verificationCodeController.text.isNotEmpty)
                            //TODO Register Phone Number Method
                          });
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
      );
    else
      return SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(45,60,45,0),
          child: Center(
            child: IntrinsicWidth(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  Center(
                    child: Text(
                      'جهت ثبت نام موارد زیر را کامل کنید',
                      style: TextStyle(
                          fontSize: 25,
                          fontWeight: FontWeight.bold
                      ),
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
                                  color: Colors.white
                              ),
                              keyboardType: TextInputType.phone,
                              decoration: InputDecoration(
                                enabledBorder: OutlineInputBorder(
                                  borderSide: BorderSide(color: Colors.white, width: 2.0),
                                ),
                                focusedBorder: OutlineInputBorder(
                                  borderSide: BorderSide(color: Colors.white, width: 2.0),
                                ),
                                border: OutlineInputBorder(),
                                labelStyle: TextStyle(color: Colors.white,),
                                labelText: 'نام کاربری',
                              ),
                              controller: userNameController,
                            ),
                          ),
                          Expanded(
                            flex: 2,
                            child: Card(
                            color: !isCheckingUserName ?
                                    Colors.red[700] : Colors.red[400],
                            child: TextButton(
                              onPressed: (){
                                setState(() async {
                                  if(!isCheckingUserName){
                                    isCheckingUserName = true;
                                    if(await isUserNameRepetitive(userNameController.text))
                                      Fluttertoast.showToast(msg:
                                      'نام کاربری تکراری است. لطفا آن را تغییر دهید');
                                    else
                                      Fluttertoast.showToast(msg:
                                      'نام کاربری در دسترس است');
                                    isCheckingUserName = false;
                                  }
                                });
                              },
                              child: Text((!isCheckingUserName) ?
                              'بررسی کن' : 'در حال بررسی',
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
                              style: TextStyle(
                                  color: Colors.white
                              ),
                              keyboardType: TextInputType.phone,
                              decoration: InputDecoration(
                                border: OutlineInputBorder(),
                                enabledBorder: OutlineInputBorder(
                                  borderSide: BorderSide(color: Colors.white, width: 2.0),
                                ),
                                focusedBorder: OutlineInputBorder(
                                  borderSide: BorderSide(color: Colors.white, width: 2.0),
                                ),
                                labelText: 'رمز عبور',
                                labelStyle: TextStyle(color: Colors.white,),
                              ),
                              controller: passwordController,
                            ),
                          ),
                        ),
                        Expanded(
                          child: Padding(
                            padding: const EdgeInsets.only(right: 4.0),
                            child: TextField(
                              style: TextStyle(
                                  color: Colors.white
                              ),
                              keyboardType: TextInputType.phone,
                              decoration: InputDecoration(
                                border: OutlineInputBorder(),
                                enabledBorder: OutlineInputBorder(
                                  borderSide: BorderSide(color: Colors.white, width: 2.0),
                                ),
                                focusedBorder: OutlineInputBorder(
                                  borderSide: BorderSide(color: Colors.white, width: 2.0),
                                ),
                                labelText: 'تکرار رمز عبور',
                                labelStyle: TextStyle(color: Colors.white,),
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
                      onPressed: (){
                        setState(() {
                          userNameError = passwordError = '';
                          if(userNameController.text.isEmpty)
                            userNameError = 'نام کاربری الزامی است';
                          if(passwordController.text.isEmpty)
                            passwordError = 'رمز عبور الزامی است';
                          else if(confirmPasswordController.text.isEmpty ||
                            passwordController.text != confirmPasswordController.text)
                            passwordError = 'رمز عبور مطابقت ندارد';
                          // if(phoneNumberController.text.isNotEmpty &&
                          // verificationCodeController.text.isNotEmpty)
                          //TODO SignUp Method
                        });
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
      );

  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: authForm(formName),
      persistentFooterButtons: <Widget>[
        SizedBox(
          width: MediaQuery.of(context).size.width / 2 - 12,
          child: Card(
            color: formName == FormName.SignIn  ?
            Color(0xFF20BFA9) : Color(0xFF202028),
            child: TextButton(
              onPressed: (){
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
                    formName == FormName.RegisterPhoneNumber) ?
            Color(0xFF20BFA9) : Color(0xFF202028),
            child: TextButton(
              onPressed: (){
                setState(() {
                  formName = FormName.SignUp;
                });
              },
              child: Text(formName == FormName.RegisterPhoneNumber ?
                'ثبت شماره همراه' : 'ثبت نام',
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