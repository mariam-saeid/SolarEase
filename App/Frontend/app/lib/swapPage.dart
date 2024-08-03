import 'package:app/Login.dart';
import 'package:app/SignUp.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';

import 'package:flutter_circular_text/circular_text.dart';
double scw = 0; // 60% of screen width
double sch = 0;
double t = 0;
double width(double w){return scw * (w /783);}
double height(double h){return (h / 393) * sch;}

class SwapPaget extends StatelessWidget {
  const SwapPaget({super.key});

  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width; 
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;
    return  WillPopScope(onWillPop: () async {
      // Return false to disable the back button
      return false;
    },child:  SwapToLoginPage()); 
  }
}
//   -----------------------------------------------------

class SwapToLoginPage extends StatefulWidget {
  const SwapToLoginPage({super.key});

  @override
  State<SwapToLoginPage> createState() => _SwapToLoginPageState();
}

class _SwapToLoginPageState extends State<SwapToLoginPage> {
  @override
  Widget build(BuildContext context) {
    
    return GestureDetector(
      
      onVerticalDragEnd: (DragEndDetails details) {
        if (details.primaryVelocity! > 0) { 
           Navigator.pushReplacement(
            context,
            MaterialPageRoute(
                builder: (context) =>SignUpPage() ),
          );
         
        }
      },
      child: Scaffold(
        
        body: Column(
          children: <Widget>
          [
            Align(
            alignment: Alignment.topRight,
            child: Image.asset(
              'assets/SwapPage/upper.png',
              width: width(370),
              height: height(60),
              alignment: Alignment.centerRight,
              fit: BoxFit.fill,
            ),
          ),
          Spacer(flex: 1,),
        //------------ The body-------------------------------------------
            
            Stack(children: 
            [
              Image.asset('assets/SwapPage/solar.jpeg',width: width(550.0),height: height(140.0),),
                  
              Padding(
                padding: const EdgeInsets.only(left: 16),
                child: CircularText
                (
                children: 
                [
                  TextItem
                  (
                    text: Text("Solar", //toUpperCase()
                    style: TextStyle(fontSize: 52,color: Colors.black,fontWeight: FontWeight.bold,),),
                    startAngle: 232,
                    startAngleAlignment: StartAngleAlignment.start,
                  ),
                 
                  TextItem
                  (
                    text: Text("Ease", //toUpperCase()
                    style: TextStyle(fontSize: 52,color: Colors.green,fontWeight: FontWeight.bold,),),
                    startAngle: 282,
                    startAngleAlignment: StartAngleAlignment.start,
                  ),
                   
                ],
                position: CircularTextPosition.outside,
                 backgroundPaint: null,
                ),
              ),
            
            ],),
            
      
            // -------------- swap Icon----------------------------------
      
            
           Row(
            children:[
              Spacer(flex:1),
           SizedBox(
      width: width(300),
      height: height(30),
      child: ElevatedButton(
        onPressed: (){
           Navigator.pushReplacement(
                      context,
                      MaterialPageRoute(
                          builder: (context) => SignUp()),
                    );
           
        },
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
    ),
    Spacer(flex: 1),
              SizedBox(
                width: width(300),
                height: height(30),
                child: ElevatedButton(
                  onPressed: () {
                     Navigator.pushReplacement(
                      context,
                      MaterialPageRoute(
                          builder: (context) =>Login() ),
                    );
                    
                  },
                  child: Text(
                    'Login',
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
            ]
           ),
           
           SizedBox(height:height(35)),
        
            // صورة تحت خالص ناحية الشمال
             Align(
            alignment: Alignment.bottomLeft,
            child: Image.asset(
              'assets/SwapPage/down.png',
              width: width(380),
              height: height(60),
              alignment: Alignment.centerRight,
              fit: BoxFit.fill,
            ),
          ),
          ],
        ),
      ),
    );
  }
}
