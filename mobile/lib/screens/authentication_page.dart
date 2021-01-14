import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:mobile/store/course_store.dart';
import 'package:async/async.dart';
import 'package:mobile/services/authentication_service.dart';

enum FormName{
  SignInThroughUserName,
  SignInThroughPhoneNumber,
  SignUp,
  RegisterPhoneNumber,
}

class AuthenticationPage extends StatefulWidget {

  AuthenticationPage();

  @override
  _AuthenticationPageState createState() => _AuthenticationPageState();
}

class _AuthenticationPageState extends State<AuthenticationPage> {
  var representer;
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
      if(formName == FormName.SignInThroughPhoneNumber){
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
    if(formName == FormName.SignInThroughPhoneNumber)
      return SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(45,65,45,0),
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
                Padding(
                  padding: const EdgeInsets.only(top: 28.0),
                  child: Card(
                    color: Color(0xFF20BFA9),
                    child: TextButton(
                      onPressed: (){
                        setState(() {
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
                ),
              ],
            ),
          ),
        ),
      );
    else if(formName == FormName.RegisterPhoneNumber)
      return SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(45,65,45,0),
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
                Padding(
                  padding: const EdgeInsets.only(top: 28.0),
                  child: Card(
                    color: Color(0xFF20BFA9),
                    child: TextButton(
                      onPressed: (){
                        setState(() {
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
                ),
              ],
            ),
          ),
        ),
      );
    else if (formName == FormName.SignUp)
      return SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(45,65,45,0),
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
                                    Fluttertoast.showToast(msg: 'نام کاربری تکراری است. لطفا آن را تغییر دهید');
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
                Padding(
                  padding: const EdgeInsets.only(top: 28.0),
                  child: IntrinsicHeight(
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
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 28.0),
                  child: Card(
                    color: Color(0xFF20BFA9),
                    child: TextButton(
                      onPressed: (){
                        setState(() {
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
                ),
              ],
            ),
          ),
        ),
      );
    else
      return SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(45,65,45,0),
          child: IntrinsicWidth(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: <Widget>[
                Center(
                  child: Text(
                    'نام کاربری و رمز عبور خود را وارد کنید',
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
                      labelText: 'نام کاربری',
                    ),
                    controller: userNameController,
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 28.0),
                  child: Expanded(
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
                Padding(
                  padding: const EdgeInsets.only(top: 28.0),
                  child: Card(
                    color: Color(0xFF20BFA9),
                    child: TextButton(
                      onPressed: (){
                        setState(() {
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
                ),
              ],
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
            color: (formName == FormName.SignInThroughPhoneNumber ||
                    formName == FormName.SignInThroughUserName) ?
            Color(0xFF20BFA9) : Color(0xFF202028),
            child: TextButton(
              onPressed: (){
                setState(() {
                  formName = FormName.SignInThroughUserName;
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
