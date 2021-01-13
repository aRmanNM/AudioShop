import 'package:flutter/material.dart';

enum FormName{
  SignIn,
  SignUp,
}

class LoginPage extends StatefulWidget {

  LoginPage();

  @override
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {

  var formName = FormName.SignIn;
  TextEditingController phoneNumberController = new TextEditingController();
  TextEditingController nameController = new TextEditingController();
  TextEditingController verificationCodeController = new TextEditingController();

  Widget authForm(FormName formName){
    if(formName == FormName.SignIn)
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
                          child: Card(
                            color: Colors.red,
                            child: TextButton(
                              onPressed: (){
                                setState(() {
                                  //TODO Receive Token Verification Code
                                });
                              },
                              child: Text(
                                'دریافت کد',
                                style: TextStyle(
                                  fontSize: 16,
                                  fontWeight: FontWeight.bold,
                                  color: Colors.white,
                                ),
                              ),
                            ),
                          ),
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
                    'برای ثبت نام، اطلاعات خود را وارد کنید',
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
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                      enabledBorder: OutlineInputBorder(
                        borderSide: BorderSide(color: Colors.white, width: 2.0),
                      ),
                      focusedBorder: OutlineInputBorder(
                        borderSide: BorderSide(color: Colors.white, width: 2.0),
                      ),
                      border: OutlineInputBorder(),
                      labelStyle: TextStyle(color: Colors.white,),
                      labelText: 'نام',
                    ),
                    controller: nameController,
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
                          child: Card(
                            color: Colors.red,
                            child: TextButton(
                              onPressed: (){
                                setState(() {
                                  //TODO Receive Token Verification Code
                                });
                              },
                              child: Text(
                                'دریافت کد',
                                style: TextStyle(
                                  fontSize: 16,
                                  fontWeight: FontWeight.bold,
                                  color: Colors.white,
                                ),
                              ),
                            ),
                          ),
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
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: authForm(formName),
      persistentFooterButtons: <Widget>[
        SizedBox(
          width: MediaQuery.of(context).size.width / 2 - 12,
          child: Card(
            color: formName == FormName.SignIn ?
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
            color: formName == FormName.SignUp ?
            Color(0xFF20BFA9) : Color(0xFF202028),
            child: TextButton(
              onPressed: (){
                setState(() {
                  formName = FormName.SignUp;
                });
              },
              child: Text(
                'ثبت نام',
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
