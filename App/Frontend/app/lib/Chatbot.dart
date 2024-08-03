import 'package:app/api_objects/Button.dart';
import 'package:app/api_objects/RealChat.dart';
import 'package:app/homeuser.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/gestures.dart';
import 'package:flutter/widgets.dart';

double scw = 0; // 60% of screen width
double sch = 0;
double t = 0;
double wid(double w) {
  return scw * (w / 365);
}

double hig(double h) {
  return (h / 680) * sch;
}

class Chatbot extends StatefulWidget {
  @override
  _ChatbotState createState() => _ChatbotState();
}

/***********************************************************************************************/
//main widget
class _ChatbotState extends State<Chatbot> {
  bool tabed=false;
  bool NoError = true;
  bool chatloading = true;
  List<String> _messages = []; //chat of user
  List<Button> buttons = [];
  List<Widget> _menuButtons = []; //meanu of buttons
  ScrollController _scrollController = ScrollController();
  TextEditingController _messageController = TextEditingController();
  bool insideSecond = false;


  /***********************************************************************************************/
  //when click on specific message call specigic function
  void convert() async {
    print(buttons);
    print(insideSecond);
    _menuButtons.clear();
    if (insideSecond) {
      /**********************inside and not back****************************/
      for (Button b in buttons) {
        if (b.message != "back") {
          print("i am  inside and not back");
          /*******for each button********/   
          _menuButtons.add(ElevatedButton(
           /*******add it to _menu Buttons********/
            onPressed: () async {
             
              /***********add question that i click on to chat************/
              _messages.add("You: " + b.message);
              setState(() {
                chatloading = true;
              });
              /*************call api for answer and add it to chat****************/
              NoError = await clickOnQuestion(_messages, buttons, b.id);
             
              setState(() {
                insideSecond = true;
                chatloading = false;
              });
            },
            child: Text(
              b.message,
              style: TextStyle(color: Colors.white),
            ),
            style: ElevatedButton.styleFrom(
              backgroundColor: Color(0x0ffED9555),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(30),
              ),
            ),
          ));
        }
        /**********************inside and  back****************************/
        else if (insideSecond && b.message == "back") {
        
          _menuButtons.add(ElevatedButton(
            
            onPressed: () async {
              setState(() {
                insideSecond = false;
                chatloading = true;
              
              });
              /**********return first _menue***********/
              NoError = await backClick(buttons);
              setState(() {
                chatloading = false;
              });
            },
            child: Text(
              b.message,
              style: TextStyle(color: Colors.white),
            ),
            style: ElevatedButton.styleFrom(
              backgroundColor: Color(0x0ffED9555),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(30),
              ),
            ),
          ));
        }
      }
    }

    /**********************not inside****************************/
    else if (!insideSecond) {
      for (Button b in buttons) {
       
        _menuButtons.add(ElevatedButton(
          
          onPressed: () async {
            setState(() {
              insideSecond = true;
              chatloading = true;
            });
            /*********return second menue************/
            NoError = await clikmain(buttons, b.id);
            setState(() {
              insideSecond = true;
              chatloading = false;
            });
          },
          child: Text(
            b.message,
            style: TextStyle(color: Colors.white),
          ),
          style: ElevatedButton.styleFrom(
            backgroundColor: Color(0x0ffED9555),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(30),
            ),
          ),
        ));
      }
    }
  }

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance?.addPostFrameCallback((_) {
      _scrollToBottom();
    });
    getchat();
  }

  Future<void> getchat() async {
    /**********return chat and questions of main menu************/
    NoError = await initialchat(_messages, buttons);
    setState(() {
      chatloading = false;
    });
  }

  void _scrollToBottom() {
    _scrollController.animateTo(
      _scrollController.position.maxScrollExtent,
      duration: Duration(milliseconds: 300),
      curve: Curves.easeOut,
    );
  }

/*******************api for send text*******************/
  Future<void> _sendMessage() async {
    String message = _messageController.text.trim();
    if (message.isNotEmpty) {
      _messages.add('You: $message');
      _messageController.clear();
      setState(() {
        chatloading = true;
      });

      String answer = await sendText(message);

      if (answer != "Error") {
        _messages.add("Bot: " + answer.replaceAll(';', '\n'));
        ;
      } else {}
      setState(() {
        chatloading = false;
      });
    }
  }

  @override
  void dispose() {
    _scrollController.dispose();
    super.dispose();
  }

  ScrollController scrollController = ScrollController();
  
/********************************************** ui of page *****************************************/
  @override
  Widget build(BuildContext context) {
    convert();
    scrollController = ScrollController();

    scw = MediaQuery.of(context).size.width; // 60% of screen width
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;
    return  WillPopScope(
      onWillPop: () async {
        // Return false to disable the back button
        return false;
      },
      child: Scaffold(
        body: Stack(
          children: [
            SingleChildScrollView(
              child: Center(
                  child: Column(
                children: [
                  SizedBox(
                    height: hig(100),
                    width: double.infinity,
                    child: Stack(
                      children: [
                        Positioned(
                          top: 0,
                          right: 0,
                          child: Image.asset(
                            'assets/Chatbot/cloud.png',
                            fit: BoxFit.fill,
                            width: wid(187.5),
                            height: 90,
                          ),
                        ),
                        Column(
                          children: [
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              // Align items vertically
                              children: [
                                GestureDetector(
                                  onTap: () {
                                    Navigator.pushReplacement(
                                      context,
                                      MaterialPageRoute(
                                          builder: (context) => HomeUser(userr: user,)),
                                    );
      
                                    //  Navigator.pop(context);
                                    print('Arrow tapped');
                                  },
                                  child: Image.asset(
                                    'assets/Chatbot/arrow.png',
                                    width: wid(94),
                                    height: hig(80),
                                  ),
                                ),
                                // Add space between the icon and text
                                Text(
                                  'Chatbot',
                                  style: TextStyle(
                                    fontSize: 25.0 * t,
                                    fontWeight: FontWeight.bold,
                                    color: Color(0x0ff063221),
                                  ),
                                ),
                                SizedBox(
                                  width: wid(94),
                                ),
                                // To take all remaining space
                              ],
                            ),
                            if (!NoError)
                              Center(
                                child: Text(
                                  "Oops! Something went wrong.",
                                  style: TextStyle(
                                      fontSize: 15.0, color: Colors.red),
                                ),
                              )
                          ],
                        ),
                      ],
                    ),
                  ),
                  SizedBox(
                    height:tabed?hig(420):hig(515),//440
                    width: double.infinity,
                    child: Stack(
                      children: [
                        Center(
                          child: Image.asset(
                            'assets/Chatbot/bot3.png',
                            width: wid(250),
                            height: hig(290), //300
                          ),
                        ),
                        //for display user and bot messages
                        SingleChildScrollView(
                          controller: _scrollController,
                          reverse: true,
                          child: ListView.builder(
                            shrinkWrap: true,
                            physics: ClampingScrollPhysics(),
                            itemCount: _messages.length,
                            itemBuilder: (_, int index) {
                              String message = _messages[index];
                              bool isBotMessage = message.startsWith('Bot:');
      
                              message = message.substring(5);
      
                              bool isFirstMessageOfBatch = index == 0 ||
                                  !_messages[index - 1]
                                      .startsWith(isBotMessage ? 'Bot:' : 'You:');
                              Image iconData = isBotMessage
                                  ? Image.asset('assets/Chatbot/bot.png',
                                      width: wid(30), height: hig(30))
                                  : Image.network(user.image,
                                      width: wid(30), height: hig(30));
      
                              return ListTile(
                                title: Column(
                                  crossAxisAlignment: isBotMessage
                                      ? CrossAxisAlignment.start
                                      : CrossAxisAlignment.end,
                                  children: [
                                    if (isFirstMessageOfBatch) // Display icon only for the first message of each batch
                                      iconData,
                                    SizedBox(height: hig(4)),
                                    ConstrainedBox(
                                      constraints: BoxConstraints(
                                        maxWidth: wid(
                                            300), // Set the maximum width here
                                      ),
                                      child: Container(
                                        padding: EdgeInsets.all(8.0),
                                        decoration: BoxDecoration(
                                          color: isBotMessage
                                              ? Colors.blueGrey[100]
                                              : Colors.green[100],
                                          borderRadius:
                                              BorderRadius.circular(8.0),
                                        ),
                                        child: Text(message),
                                      ),
                                    ),
                                  ],
                                ),
                                contentPadding:
                                    EdgeInsets.symmetric(vertical: hig(2.0)),
                              );
                            },
                          ),
                        )
                      ],
                    ),
                  ),
                  Row(
                    children: [
                      Spacer(flex: 1),
                      GestureDetector(
                          onTap: () {
                            setState(() {
                              tabed = !tabed;
                            });
                          },
                          child:tabed? Icon(Icons.arrow_circle_down_rounded,
                                  color: Color(0x0ffED9555))
                              : Icon(Icons.arrow_circle_up_rounded,color: Color(0x0ffED9555),))
                    ],
                  ),
                  SizedBox(
                      height:tabed? hig(140):hig(40),
                      child: Container(
                        width: double.infinity,
                        decoration: BoxDecoration(
                          color: Color(0x0ffBCDFA1),
                          borderRadius: BorderRadius.circular(
                              20), // Set radius to half of width/height for a perfect circle
                        ),
                        child: Column(
                          children: [
                            /***********text field************/
                            if(!tabed)inputmessage(),
                            /********************buttons********************/
                            if(tabed)fixedQuestions()
                          ],
                        ),
                      )),
                ],
              )),
            ),
            if (chatloading)
              const Center(
                child: CircularProgressIndicator(),
              )
          ],
        ),
      ),
    );
  }

/*************text field******************/
  Widget inputmessage() {
    return Center(
      child: SizedBox(
        height: hig(40),
        width: wid(350),
        // to display textfield for sending messages
        child: Row(
          children: [
            Expanded(
              child: TextField(
                controller: _messageController,
                decoration: InputDecoration(
                  hintText: 'Type a message',
                ),
              ),
            ),
            IconButton(
              iconSize: 30,
              color: Color(0x0ffED9555),
              icon: Icon(Icons.send),
              onPressed: _sendMessage,
            ),
          ],
        ),
      ),
    );
  }

/******************buttons*********************/
  Widget fixedQuestions() {
    return Center(
      child: SizedBox(
        height: hig(140),
        width: wid(250),
        // to display buttons verticaly and scrollable

        child: Scrollbar(
          thumbVisibility: true,
          controller: scrollController,
          child: SingleChildScrollView(
            controller: scrollController,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: _menuButtons.map((button) {
                return Padding(
                    padding: EdgeInsets.symmetric(vertical: hig(8.0)),
                    child: button);
              }).toList(),
            ),
          ),
        ),
      ),
    );
  }
}
