import 'dart:ui';
import 'package:app/Login.dart';
import 'package:app/ValidationCode.dart';
import 'package:app/api_objects/forgetpassword.dart';
import 'package:flutter/material.dart';



/*********************Global variables************************/

class Forgetpassword extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return
        // MaterialApp(
        //   home:
        ForgetPage();
    // );
  }
}

/*********************main widget*****************************/
class ForgetPage extends StatefulWidget {
  @override
  _ForgetPageState createState() => _ForgetPageState();
}

class _ForgetPageState extends State<ForgetPage> {
  bool isvalidforget = false;
  bool _isloadingforget = false;
  bool clickedforget = false;
  String errorforget = "Please Enter All Field";

  String email = "";

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

    return  WillPopScope(
      onWillPop: () async {
        // Return false to disable the back button
        return false;
      },
      child: GestureDetector(
        child: Scaffold(
          //  resizeToAvoidBottomInset: false,
          body: Stack(
            children: [
              SingleChildScrollView(
                child: Column(
                  children: <Widget>[
                    /****************Top right Image**************/
                    TopRightImage(wid: wid(190), hig: hig(76.96)),
                    SizedBox(
                      height: hig(30),
                    ), //Spacer(flex: 1),
                    /****************Center Image**************/
                    CenterImage(wid: wid(300), hig: hig(190)),
                    /****************"Welcome text**************/
                    TitleText(t: t),
                    SizedBox(height: hig(14.8)),
                    /****************login form**************/
                    ForgetPasswordForm(),
                    /********if there is error*******/
                    !isvalidforget && clickedforget
                        ? SizedBox(height: hig(5))
                        : SizedBox(height: hig(14.8)),
                    /**********Wan to login text***********/
                    wantLogin(t: t),
                    SizedBox(
                      height: hig(60),
                    ),
                    /**********bottom left image***********/
                    BottomLeftImage(wid: wid(190), hig: hig(76.96)),
                  ],
                ),
              ),
              if (_isloadingforget)
                const Opacity(
                    opacity: 0.8,
                    child: ModalBarrier(dismissible: false, color: Colors.black)),
              if (_isloadingforget)
                const Center(
                  child: CircularProgressIndicator(),
                )
            ],
          ),
        ),
      ),
    );
  }

  /*********************function happen when click on login*********************/
  Future<void> handleContinue() async {
    /*******if data empty*********/
    if (email == "") {
      setState(() {
        errorforget = "Please Enter Email";
        isvalidforget = false;
        clickedforget = true;
      });
    }
    /********if all data entered*********/
    else {
      setState(() {
        _isloadingforget = true;
      });

      /********Api called*********/
      var response = await checkEmail(email);

      
      setState(() {
        _isloadingforget = false;
      });

      /*********if response Ok************/
      if (response == 'Ok') {
       
        setState(() {
          FocusScope.of(context).unfocus();
          clickedforget = isvalidforget = true;
          Navigator.pushReplacement(
            context,
            MaterialPageRoute(builder: (context) => validatePage(email: email)),
          );

        

          //reset for new user
          clickedforget = false;
          isvalidforget = false;
          _isloadingforget = false;
          clickedforget = false;
          errorforget = "";
        });
      }
      /*******if response return error******/
      else {       
        setState(() {
          clickedforget = true;
          isvalidforget = false;
          errorforget = response;
        });
      }
    }
  }

  Widget ForgetPasswordForm() {
    return Container(
      child: Column(
        children: <Widget>[
          /*********enter type*********/
          buildTextFieldRow(
            icon: Icons.person,
            label: '  Email',
            width: wid(228),
            height: hig(32.56),
            onChanged: (value) => email = value,
          ),
          SizedBox(height: hig(14.8)),

          /*********login button*********/
          SizedBox(
            width: wid(200),
            child: ElevatedButton(
              onPressed: handleContinue,
              child: Text(
                'Continue',
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
          if (!isvalidforget && clickedforget) builderrorinputtext(errorforget),
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
      'assets/ForgetPassword/forgetpassword2.png',
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
      'Forget Password',
      style: TextStyle(
        fontSize: 25.0 * t,
        fontWeight: FontWeight.bold,
        color: Color(0xff063221),
      ),
    );
  }
}


/********************widget for "Forget password" text*********************/
class wantLogin extends StatelessWidget {
  final double t;
  const wantLogin({required this.t});

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: (){
          Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => Login()),
        );
      },
      child: Text(
        'Do you want to login ?',
        style: TextStyle(
          fontSize: 15.0 * t,
          color: Colors.grey,
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
