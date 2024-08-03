import 'package:app/ForgetPassword.dart';
import 'package:app/SignUp.dart';
import 'package:app/api_objects/user.dart';
import 'package:app/filterpost.dart';
import 'package:flutter/material.dart';
import 'homeuser.dart';

/*********************Global variables************************/
bool isvalid = false;
bool _isloading = false;
bool clicked = false;
String type = '';
String error = "Please Enter All Field";

User userlogin = User(
    username: "username",
    password: "",
    email: "",
    location: "location",
    phoneNumber: "phoneNumber",
    city: "city");

class Login extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return
        WillPopScope(
            onWillPop: () async {
              return false;
            }, child: LoginPage());
  }
}

/*********************main widget*****************************/
class LoginPage extends StatefulWidget {
  @override
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  late double scw;
  late double sch;
  late double t;
  double wid(double w) {
    return scw * (w / 380);
  }

  double hig(double h) {
    return (h / 592) * sch;
  }

  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width;
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;

    return GestureDetector(
      child: Scaffold(
        body: Stack(
          children: [
            SingleChildScrollView(
              child: Column(
                children: <Widget>[
                  /****************Top right Image**************/
                  TopRightImage(wid: wid(190), hig: hig(76.96)),
                  SizedBox(
                    height: hig(10),
                  ), 
                  /****************Center Image**************/
                  CenterImage(wid: wid(266), hig: hig(148)),
                  /****************"Welcome text**************/
                  TitleText(t: t),
                  SizedBox(height: hig(14.8)),
                  /****************login form**************/
                  LoginForm(wid: wid, hig: hig, onPressed: handleLogin),
                  /********if there is error*******/
                  !isvalid && clicked
                      ? SizedBox(height: hig(5))
                      : SizedBox(height: hig(14.8)),
                  /**********Forget password text***********/
                  ForgotPasswordText(t: t),
                  SizedBox(height: hig(5)), 
                  /************"Want to create account?" text***********/
                  SignUpText(t: t),
                  /**********bottom left image***********/
                  BottomLeftImage(wid: wid(190), hig: hig(76.96)),
                ],
              ),
            ),
            if (_isloading)
              const Opacity(
                  opacity: 0.8,
                  child: ModalBarrier(dismissible: false, color: Colors.black)),
            if (_isloading)
              const Center(
                child: CircularProgressIndicator(),
              )
          ],
        ),
      ),
    );
  }

  /*********************function happen when click on login*********************/
  Future<void> handleLogin() async {
    /*******if data empty*********/
    if (userlogin.email == '' || userlogin.password == '' || type == '') {
      setState(() {
        error = "Please Enter All Field";
        isvalid = false;
        clicked = true;
      });
    }
    /********if all data entered*********/
    else {
      setState(() {
        _isloading = true;
      });
      var response;

      /********Api called based on type*********/
      if (type == "User") {
        response = await userlogin.checkuser();
      } else {
        response = await userlogin.checkAdmin();
      }

      setState(() {
        _isloading = false;
      });
      /*********if response Ok************/
      if (response == 'Ok') {
        setState(() {
          FocusScope.of(context).unfocus();
          clicked = isvalid = true;

          final User u = User(
            username: userlogin.username,
            password: '',
            email: userlogin.email,
            location: userlogin.location,
            phoneNumber: userlogin.phoneNumber,
            city: userlogin.city,
          );
          u.id = userlogin.id;
          u.systemsize = userlogin.systemsize;
          u.useKey = userlogin.useKey;
          u.image = userlogin.image;

          if (type == 'User') {
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) => HomeUser(userr: u)),
            );

           
          } else {
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) => FilterPost(u: u)),
            );

            
          }

          //reset for new user
          userlogin.email = '';
          userlogin.password = '';
          userlogin.username = '';
          userlogin.email = '';
          
          clicked = false;
          isvalid = false;
          _isloading = false;
          clicked = false;
          error = "";
          type = '';
        });
      }
      /*******if response return error******/
      else {
        setState(() {
          clicked = true;
          isvalid = false;
          error = response;
        });
      }
    }
  }
}

/******************widget for top right image**********************/
class TopRightImage extends StatelessWidget {
  final double wid;
  final double hig;

  const TopRightImage({required this.wid, required this.hig});

  @override
  Widget build(BuildContext context) {
    return Align(
      alignment: Alignment.topRight,
      child: Image.asset(
        'assets/Login/4.png',
        width: wid,
        height: hig,
        alignment: Alignment.centerRight,
        fit: BoxFit.fill,
      ),
    );
  }
}

/*******************widget for centeral image*********************/
class CenterImage extends StatelessWidget {
  final double wid;
  final double hig;

  const CenterImage({required this.wid, required this.hig});

  @override
  Widget build(BuildContext context) {
    return Image.asset(
      'assets/Login/1.png',
      width: wid,
      height: hig,
    );
  }
}

/***************widget for "Welcome" text**********************/
class TitleText extends StatelessWidget {
  final double t;

  const TitleText({required this.t});

  @override
  Widget build(BuildContext context) {
    return Text(
      'Welcome',
      style: TextStyle(
        fontSize: 25.0 * t,
        fontWeight: FontWeight.bold,
        color: Color(0xff063221),
      ),
    );
  }
}

/******************Login form***********************/
class LoginForm extends StatefulWidget {
  final double Function(double) wid;
  final double Function(double) hig;
  final VoidCallback onPressed;

  LoginForm({
    required this.wid,
    required this.hig,
    required this.onPressed,
  });

  @override
  State<LoginForm> createState() => _LoginFormState();
}

class _LoginFormState extends State<LoginForm> {
  @override
  Widget build(BuildContext context) {
    return Container(
      child: Column(
        children: <Widget>[
          /*********Select type*********/
          buildDropdownRow(
            icon: Icons.check,
            label: '  Select Type',
            width: widget.wid(228),
            height: widget.hig(45),
          ),
          SizedBox(height: widget.hig(14.8)),
          /*********enter type*********/
          buildTextFieldRow(
            icon: Icons.person,
            label: '  Email',
            width: widget.wid(228),
            height: widget.hig(32.56),
            onChanged: (value) => userlogin.email = value,
          ),
          SizedBox(height: widget.hig(14.8)),
          /*********Select password*********/
          buildTextFieldRow(
            icon: Icons.password,
            label: '  Password',
            width: widget.wid(228),
            height: widget.hig(32.56),
            onChanged: (value) => userlogin.password = value,
          ),
          SizedBox(height: widget.hig(14.8)),
          /*********login button*********/
          SizedBox(
            width: widget.wid(200),
            child: ElevatedButton(
              onPressed: widget.onPressed,
              child: Text(
                'LogIn',
                style: TextStyle(
                  fontSize: 15,
                  color: Colors.white,
                  fontWeight: FontWeight.bold,
                ),
              ),
              style: ElevatedButton.styleFrom(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(20),
                ),
                backgroundColor: Color(0xff063221),
              ),
            ),
          ),
          /*******if data not valid and click******/
          if (!isvalid && clicked) builderrorinputtext(error),
        ],
      ),
    );
  }

  /****************Widget build red error message******************/
  Widget builderrorinputtext(String e) {
    return Text(
      e,
      style: TextStyle(color: Colors.red, fontSize: 13),
    );
  }

  /*********************Widget for dropdoenlist (type)**********************/
  Widget buildDropdownRow({
    required IconData icon,
    required String label,
    required double width,
    required double height,
  }) {
    return Row(
      children: [
        Spacer(flex: 2),
        Icon(icon, color: Color(0xff063221)),
        Spacer(flex: 1),
        SizedBox(
            width: width,
            height: height,
            child: DropdownButtonFormField<String>(
              value: type != '' ? type : null,
              hint: Text(
                label,
                style: TextStyle(color: Colors.grey, fontSize: 14),
              ),
              items: ['User', "Admin"]
                  .map<DropdownMenuItem<String>>((String value) {
                return DropdownMenuItem<String>(
                    value: value,
                    child: Text(value, style: TextStyle(fontSize: 14)));
              }).toList(),
              decoration: InputDecoration(
                filled: true,
                fillColor: Color.fromARGB(255, 226, 234, 221),
                enabledBorder: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(20),
                  borderSide: BorderSide(color: Colors.transparent),
                ),
                focusedBorder: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(20),
                  borderSide: BorderSide(color: Colors.transparent),
                ),
              ),
              onChanged: (value) {
                setState(() {
                  type = value!;
                  print(type);
                });
              },
            )),
        Spacer(flex: 2),
      ],
    );
  }

  /*****************widget for text field input********************/
  Widget buildTextFieldRow({
    required IconData icon,
    required String label,
    required double width,
    required double height,
    required Function(String) onChanged,
  }) {
    return Row(
      children: [
        Spacer(flex: 2),
        Icon(icon, color: Color(0xff063221)),
        Spacer(flex: 1),
        SizedBox(
          width: width,
          height: height,
          child: TextField(
            
            obscureText: label == "  Password" ? true : false,
            decoration: InputDecoration(
              contentPadding: EdgeInsets.zero,
              labelStyle: TextStyle(color: Colors.grey),
              labelText: label,
              filled: true,
              fillColor: Color.fromARGB(255, 226, 234, 221),
              enabledBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(20),
                borderSide: BorderSide(color: Colors.transparent),
              ),
              focusedBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(20),
                borderSide: BorderSide(color: Colors.transparent),
              ),
            ),
            onChanged: onChanged,
          ),
        ),
        Spacer(flex: 2)
      ],
    );
  }
}

/********************widget for "Forget password" text*********************/
class ForgotPasswordText extends StatelessWidget {
  final double t;
  const ForgotPasswordText({required this.t});

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => Forgetpassword()),
        );
       
      },
      child: Text(
        'forget Pssword ?',
        style: TextStyle(
          fontSize: 15.0 * t,
          color: Colors.grey,
        ),
      ),
    );
  }
}

/****************widget for "Don't have an accout ?" text********************/
class SignUpText extends StatelessWidget {
  final double t;
  const SignUpText({required this.t});

  @override
  Widget build(BuildContext context) {
    return Align(
      alignment: Alignment.centerRight,
      child: Padding(
        padding: EdgeInsets.only(right: 9.5),
        child: GestureDetector(
          onTap: () {
            FocusScope.of(context).unfocus();
            userlogin.email = '';
            userlogin.password = '';
            clicked = false;
            isvalid = false;
            _isloading = false;
            error = "";
            type = '';
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) => SignUp()),
            );

           
          },
          child: Text(
            "Don't have an accout ?",
            style: TextStyle(
              fontSize: 15.0 * t,
              color: Colors.grey,
            ),
          ),
        ),
      ),
    );
  }
}

/******************widget for most left bottom image*********************/
class BottomLeftImage extends StatelessWidget {
  final double wid;
  final double hig;

  const BottomLeftImage({required this.wid, required this.hig});

  @override
  Widget build(BuildContext context) {
    return Align(
      alignment: Alignment.bottomLeft,
      child: Image.asset(
        'assets/Login/3.png',
        width: wid,
        height: hig,
        alignment: Alignment.centerRight,
        fit: BoxFit.fill,
      ),
    );
  }
}
