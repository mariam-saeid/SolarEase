import 'package:app/Login.dart';
import 'package:app/api_objects/user.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

/*****************Global variables****************************/
bool validData = false;
bool clicked = false;
bool _isloading = false;
List<String> cities = [
  "Cairo",
  "Alexandria",
  "Aswan",
  "Asyut",
  "Beheira",
  "Beni Suef",
  "Dakahlia",
  "Damietta",
  "Faiyum",
  "Gharbia",
  "Giza",
  "Ismailia",
  "Kafr el-Sheikh",
  "Marsa Matruh",
  "Minya",
  "Monufia",
  "New Valley",
  "North Sina",
  "Port Said",
  "Qalyubia",
  "Qena",
  "Red Sea",
  "Sharqia",
  "Sohag",
  "South Sina",
  "Suez",
  "Luxor"
];
class SignUp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return  WillPopScope(
        onWillPop: () async {
          // Return false to disable the back button
          return false;
        }, child: SignUpPage());
  }
}

/*****************************main widget******************************/
class SignUpPage extends StatefulWidget {
  SignUpPage();
  @override
  _SignUpPageState createState() => _SignUpPageState();
}

class _SignUpPageState extends State<SignUpPage> {
  late double scw;
  late double sch;
  late double t;
  double wid(double w) {
    return scw * (w / 380);
  }

  double hig(double h) {
    return (h / 592) * sch;
  }

  /******************Create a User object to hold form data******************/
  final User user = User(
      username: '',
      password: '',
      email: '',
      location: '',
      phoneNumber: '',
      city: '');

  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width;
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;

    return Scaffold(
       body:Stack(
      children: [
          SingleChildScrollView(
            child: Column(
              children: <Widget>[
                /********Top image***************/
                TopImage(wid: wid(190), hig: hig(76.96)),
                SizedBox(height: hig(20),),//Spacer(flex: 1),
                /********Title text*************/
                TitleText(t: t),
                SizedBox(height: hig(14.8)),
                /*******Sign-up form***********/
                SignUpForm(
                    user: user,
                    wid: wid,
                    hig: hig,
                    t: t,
                    setstateofPage: setstateofPage),
                  SizedBox(
                  height: hig(20),
                ), //Spacer(flex: 1),
                /*******Bottom image*******/
                BottomImage(wid: wid(190), hig: hig(76.96)),
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
    ),);
  }

  void setstateofPage() {
    setState(() {});
  }
}

/*************************Widget for the top image**************************/
class TopImage extends StatelessWidget {
  final double wid;
  final double hig;
  const TopImage({required this.wid, required this.hig});

  @override
  Widget build(BuildContext context) {
    return Align(
      alignment: Alignment.topRight,
      child: Image.asset(
        'assets/signup/4.png',
        width: wid,
        height: hig,
        alignment: Alignment.centerRight,
        fit: BoxFit.fill,
      ),
    );
  }
}

/************************Widget for the bottom image***************************/
class BottomImage extends StatelessWidget {
  final double wid;
  final double hig;

  const BottomImage({required this.wid, required this.hig});

  @override
  Widget build(BuildContext context) {
    return Align(
      alignment: Alignment.bottomLeft,
      child: Image.asset(
        'assets/signup/3.png',
        width: wid,
        height: hig,
        alignment: Alignment.centerRight,
        fit: BoxFit.fill,
      ),
    );
  }
}

/************************Widget for the title text************************/
class TitleText extends StatelessWidget {
  final double t;
  const TitleText({required this.t});

  @override
  Widget build(BuildContext context) {
    return Text(
      'Create Your Account',
      style: TextStyle(
        fontSize: 25.0 * t,
        fontWeight: FontWeight.bold,
        color: Color(0xff063221),
      ),
    );
  }
}

/************************Widget for the sign-up form************************/
class SignUpForm extends StatefulWidget {
  final void Function() setstateofPage;
  final User user;
  final double Function(double) wid;
  final double Function(double) hig;
  final double t;

  const SignUpForm({
    required this.setstateofPage,
    required this.user,
    required this.wid,
    required this.hig,
    required this.t,
  });

  @override
  State<SignUpForm> createState() => _SignUpFormState();
}

class _SignUpFormState extends State<SignUpForm> {
  String error = "Please Enter All Field";
  String selectedCity = '';
  @override
  Widget build(BuildContext context) {
    return Column(
      children: <Widget>[
        /********Username field**********/
        buildTextFieldRow(
          icon: Icons.person,
          label: '  Username',
          onChanged: (value) => widget.user.username = value,
          width: widget.wid(228),
          height: widget.hig(32.56),
        ),
        SizedBox(height: widget.hig(14.8)),
        /*******Password field**********/
        buildTextFieldRow(
          icon: Icons.password,
          label: '  Password',
          obscureText: true,
          onChanged: (value) => widget.user.password = value,
          width: widget.wid(228),
          height: widget.hig(32.56),
        ),
        SizedBox(height: widget.hig(14.8)),
        /********Email field*********/
        buildTextFieldRow(
          icon: Icons.email,
          label: '  Email',
          onChanged: (value) => widget.user.email = value,
          width: widget.wid(228),
          height: widget.hig(32.56),
        ),
        SizedBox(height: widget.hig(14.8)),
        /*******Location field*******/
        buildTextFieldRow(
          icon: Icons.location_on,
          label: '  Location',
          onChanged: (value) => widget.user.location = value,
          width: widget.wid(228),
          height: widget.hig(32.56),
        ),
        SizedBox(height: widget.hig(14.8)),
        /*******Phone Number field********/
        buildTextFieldRow(
          icon: Icons.phone,
          label: '  Phone Number',
          onChanged: (value) => widget.user.phoneNumber = value,
          width: widget.wid(228),
          height: widget.hig(32.56),
        ),
        SizedBox(height: widget.hig(14.8)),
        /***********City field***********/
        buildDropdownRow(
            icon: Icons.location_city_sharp,
            label: "  Select City",
            width: widget.wid(228),
            height: widget.hig(45),
            cities: cities,
            ),
        SizedBox(height: widget.hig(14.8)),
        /***********Sign-up button*********/
        SignUpButton(
          wid: widget.wid(152),
          hig: widget.hig(32.56),
          onPressed: () {
            handleSignUp();
          },
        ),
        /**********if click and data not valid show error***********/
        !validData & clicked
            ? builderrorinputtext(error)
            : SizedBox(height: widget.hig(14.8)),
        // Login text
        LoginText(t: widget.t),
      ],
    );
  }

  /*******************Function to handle the sign-up button press***********************/
  Future<void> handleSignUp() async {
    /**********if there is fiels empty***********/
    if (widget.user.isempty()) {
      setState(() {
        error = "Please Enter All Fields";
        clicked = true;
        validData = false;
      });
    }
    /**********entered all fields ***************/
    else {
      setState(() {
        _isloading = true;
        widget.setstateofPage();
      });

      var response = await widget.user.createuse();

      setState(() {
        _isloading = false;
        widget.setstateofPage();
      });
      /**********if data ok*************/
      if (response == "Ok") {
        setState(() {
          FocusScope.of(context).unfocus();
          clicked = validData = true;
          Navigator.pushReplacement(
            context,
            MaterialPageRoute(builder: (context) =>  Login()),
          );         
          validData = false;
          clicked = false;
          _isloading = false;
        });
      }
      /************if data not valid show error*************/
      else {
        clicked = true;
        validData = false;
        error = response;
        widget.setstateofPage();
      }
    }
  }

  /*********************Widget for a text field row**********************/
  Widget buildTextFieldRow({
    required IconData icon,
    required String label,
    required double width,
    required double height,
    bool obscureText = false,
    required Function(String) onChanged,
  }) {
    return Row(
      children: [
        /*******icon******/
        Spacer(flex: 2),
        Icon(icon, color: Color(0xff063221)),
        /********text field*********/
        Spacer(flex: 1),
        SizedBox(
          width: width,
          height: height,
          child: TextField(
            keyboardType: label == "  Phone Number"
                ? TextInputType.number
                : TextInputType.text,
            inputFormatters: label == "  Phone Number"
                ? <TextInputFormatter>[FilteringTextInputFormatter.digitsOnly]
                : null,
            obscureText: obscureText,
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

/******************build dropdown list for city**************************/
  Widget buildDropdownRow({
    required IconData icon,
    required String label,
    required double width,
    required double height,
    required List<String> cities,
  }) {
    return Row(
      children: [
        /**********icon**********/
        Spacer(flex: 2),
        Icon(icon, color: Color(0xff063221)),
        /**********dropdown list**********/
        Spacer(flex: 1),
        SizedBox(
            width: width,
            height: height,
            child: DropdownButtonFormField<String>(
              value: widget.user.city != '' ? widget.user.city : null,
              hint: Text(
                label,
                style: TextStyle(color: Colors.grey,fontSize: 14),
              ),
              items: cities.map<DropdownMenuItem<String>>((String value) {
                return DropdownMenuItem<String>(
                    value: value, child: Text(value, style: TextStyle(fontSize: 14)));
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
                  widget.user.city = value!;
                  print(widget.user.city);
                });
              },
            )),
        Spacer(flex: 2),
      ],
    );
  }

  /************build red error messsage***************/
  Widget builderrorinputtext(String error) {
    return Text(
      error,
      style: TextStyle(color: Colors.red, fontSize: 13),
    );
  }
}

/********************Widget for the sign-up button***********************/
class SignUpButton extends StatelessWidget {
  final double wid;
  final double hig;
  final VoidCallback onPressed;

  const SignUpButton({
    required this.wid,
    required this.hig,
    required this.onPressed,
  });

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: wid,
      height: hig,
      child: ElevatedButton(
        onPressed: onPressed,
        child: Text(
          'Sign up',
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
    );
  }
}

/*******************Widget for the "Already have an account?" text***************************/
class LoginText extends StatelessWidget {
  final double t;
  const LoginText({required this.t});
  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
         validData = false;
         clicked = false;
         _isloading = false;
        FocusScope.of(context).unfocus();
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => Login()),
        );      
      },
      child: Text(
        'Already have an account?',
        style: TextStyle(
          fontSize: 15.0 * t,
          color: Colors.grey,
        ),
      ),
    );
  }
}