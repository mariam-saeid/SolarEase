import 'dart:ui';
import 'package:app/ForgetPassword.dart';
import 'package:app/Login.dart';
import 'package:app/ResetPassword.dart';
import 'package:app/api_objects/forgetpassword.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';

/*********************main widget*****************************/
class validatePage extends StatefulWidget {
  String email;
  validatePage({required this.email});
  @override
  _validatePageState createState() => _validatePageState();
}

class _validatePageState extends State<validatePage> {
  bool isvalidValidation = false;
  bool _isloadingValidation = false;
  bool clickedValidation = false;
  String errorValidation = "Please Enter All Field";
  String code = "";
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
                    /****************"Enter Validation Code text**************/
                    TitleText(t: t),
                    hints(t: t),
                    SizedBox(height: hig(14.8)),
                    /****************Validation code form**************/
                    ValidationForm(),
                    SizedBox(height: hig(5)),
                    /**********Want to login text***********/
                    wantLogin(t: t),
                    SizedBox(
                      height: hig(60),
                    ),
                    /**********bottom left image***********/
                    BottomLeftImage(wid: wid(190), hig: hig(76.96)),
                  ],
                ),
              ),
              if (_isloadingValidation)
                const Opacity(
                    opacity: 0.8,
                    child: ModalBarrier(dismissible: false, color: Colors.black)),
              if (_isloadingValidation)
                const Center(
                  child: CircularProgressIndicator(),
                )
            ],
          ),
        ),
      ),
    );
  }

  /*********************function happen when click on Enter*********************/
  Future<void> handleEnter() async {
    /*******if data empty*********/
    if (code == '') {
      setState(() {
        errorValidation = "Please Enter Confirmation code";
        isvalidValidation = false;
        clickedValidation = true;
      });
    }
    /********if all data entered*********/
    else {
      setState(() {
        _isloadingValidation= true;
      });
      

      /********Api called to check code*********/
      var response = await checkCode(code,widget.email);

      setState(() {
        _isloadingValidation = false;
      });
      /*********if response Ok************/
      if (response == 'Ok') {
        setState(() {

          FocusScope.of(context).unfocus();
          clickedValidation = isvalidValidation = true;
           Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) =>ResetPassword(email: widget.email)),
              );
      
        });
      }
      /*******if response return error******/
      else {
        setState(() {
          clickedValidation = true;
          isvalidValidation = false;
          errorValidation = response;
        });
      }
    }
    }
  

  /******************Validation form***********************/
  Widget ValidationForm() {
    return Container(
      child: Column(
        children: <Widget>[
          /*********enter type*********/
          buildTextFieldRow(
            icon: Icons.person,
            label: '  Confirmation code',
            width: wid(228),
            height: hig(32.56),
            onChanged: (value) => code = value,
          ),
          SizedBox(height: hig(14.8)),

          /*********login button*********/
          Row(
            children: [
              Spacer(flex: 1),
              /***************back button***************/
              SizedBox(
                width: wid(150),
                child: ElevatedButton(
                  onPressed: () {
                     Navigator.pushReplacement(
                      context,
                      MaterialPageRoute(
                          builder: (context) => Forgetpassword()),
                    );
                  },
                  child: Text(
                    'back',
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
                    backgroundColor: Color(0xffBBBAB6),
                  ),
                ),
              ),
              Spacer(flex: 1),
              /***************Enter button****************/
              SizedBox(
                width: wid(150),
                child: ElevatedButton(
                  onPressed: handleEnter,
                  child: Text(
                    'Enter',
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
              Spacer(flex: 1),
            ],
          ),
          /*******if data not valid and click******/
          if (!isvalidValidation && clickedValidation)
            builderrorinputtext(errorValidation),
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

/***************widget for "Enter Validation Code" text**********************/
class TitleText extends StatelessWidget {
  final double t;

  const TitleText({required this.t});

  @override
  Widget build(BuildContext context) {
    return Text(
      'Enter Validation Code',
      style: TextStyle(
        fontSize: 25.0 * t,
        fontWeight: FontWeight.bold,
        color: Color(0xff063221),
      ),
    );
  }
}

/******************"Enter confirmation code send at your gmail"*****************/
class hints extends StatelessWidget {
  final double t;

  const hints({required this.t});

  @override
  Widget build(BuildContext context) {
    return Text(
      'Enter Confirmation Code that sent to your mail.',
      style: TextStyle(
        fontSize: 13.0 * t,
        fontWeight: FontWeight.bold,
        color: Color(0xff5D7A6E),
      ),
    );
  }
}

/********************widget for "Do you want to login ?" text*********************/
class wantLogin extends StatelessWidget {
  final double t;
  const wantLogin({required this.t});

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
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
