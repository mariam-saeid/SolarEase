
import 'package:app/Maxload.dart';
import 'package:app/api_objects/inputCalculations.dart';
import 'package:app/homeuser.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

/********************global variables and functions******************/
Calculationsdata inpudata = Calculationsdata();
bool validData = true;
double maxloadvalueon = -1;
double maxloadvalueoff = -1;

double scw = 0;
double sch = 0;
double t = 0;
double wid(double w) {
  return scw * (w / 385);
}

double hig(double h) {
  return (h / 592) * sch;
}

class InputCalculations extends StatefulWidget {
  @override
  _InputCalculationsState createState() => _InputCalculationsState();
}

//////////////////////////////////////////////////////////////
class _InputCalculationsState extends State<InputCalculations> {
  bool _isEnabled = true;
  String Error = "";
  String dropdownValue = 'Electricity Usage';
  double _currentSliderValue = 50;

  /********************widget for grey text (info)****************************/
  Widget info(String s) {
    return Row(
      children: [
        SizedBox(width: wid(15)),
        Icon(
          Icons.info,
          color: Color(0xffBBBAB6),
          size: 18 * t,
        ),
        SizedBox(width: wid(10.7)),
        Text(
          s,
          style: TextStyle(
            color: Color(0xffBBBAB6),
            fontSize: 13 * t,
          ),
        ),
      ],
    );
  }

  /********************Main Widget*************************/
  @override
  Widget build(BuildContext context) {
    if (!_isEnabled) _currentSliderValue = 100;
    scw = MediaQuery.of(context).size.width;
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;

    return  WillPopScope(
      onWillPop: () async {
        // Return false to disable the back button
        return false;
      },
      child: Scaffold(
       
        body: SingleChildScrollView(
          child: Center(
            child: Column(
              children: <Widget>[
                /**************Hearder******************/
                Header(),
               
                /****************Switch******************/
                GridSwitch(
                  isEnabled: _isEnabled,
                  onChanged: (bool value) {
                    setState(() {
                      _currentSliderValue = 50;
                      _isEnabled = value;
                      validData = true;
                    });
                  },
                ),
      
                /**************note*********************/
                if(_isEnabled)info(
                    "Enter your electricity usage or max usage, as available."),
      
                /****************DropDown List**************************/
                if (dropdownValue == "Max usage" || !_isEnabled)
                  SizedBox(height: hig(70)),
                DropdownSection(
                  dropdownValue: dropdownValue,
                  isEnabled: _isEnabled,
                  onChanged: (String? newValue) {
                    setState(() {
                      _currentSliderValue = 50;
                      dropdownValue = newValue!;
                      validData = true;
                    });
                  },
                ),
      
                /****************Input Section**************************/
                MonthSection(
                  dropdownValue: dropdownValue,
                  isEnabled: _isEnabled,
                ),
                /*********not valid data*********/
                if (!validData) builderrorinputtext(Error),
                if (!validData) SizedBox(height: hig(5)),
      
                /********if not electricity bills**********/
                if (dropdownValue == "Max usage" || !_isEnabled)
                  info("Your maximum electricity usage per month."),
                if (dropdownValue == "Max usage" || !_isEnabled)
                SizedBox(height: hig(2),),
                if (dropdownValue == "Max usage" || !_isEnabled)
                  info("You will find it in your electricity meter reading."),
                if (dropdownValue == "Max usage" || !_isEnabled)
                  SizedBox(height: hig(50)),
      
                /*******if electricity bills*******/
                if (dropdownValue != "Max usage" && _isEnabled)
                  info("Enter your electricity usage for the last 12 months(KW)."),
      
                /*******************Slider********************************/
                ElectricalCoverageSlider(
                  isEnabled: _isEnabled,
                  currentSliderValue: _currentSliderValue,
                  onChanged: (double value) {
                    setState(() {
                      if (_isEnabled) _currentSliderValue = value;
                    });
                  },
                  dropdownValue: dropdownValue,
                ),
                /*****************if slider==0 ***************/
                if (_currentSliderValue == 0)
                  builderrorinputtext("Please Enter Value more than 0"),
                info("How much electricity that you want system to cover."),
                 SizedBox(height: hig(5),),
                if (dropdownValue == "Max usage" || !_isEnabled) SizedBox(height: hig(70)),
                /*******************next button************************/
                NextButton(onpress: onpress),
              ],
            ),
          ),
        ),
      ),
    );
  }

/****************build Error Message********************/
  Widget builderrorinputtext(String Error) {
    return Text(
      Error,
      style: TextStyle(color: Colors.red, fontSize: 13),
    );
  }

/***************When Press on next button*********************/
  void onpress() {
    /***************if Select electricity bills but wrong data***********/
    if (dropdownValue == "Electricity Usage" &&
        _isEnabled &&
        !inpudata.electricalBills.ok()) {
      setState(() {
        if (inpudata.electricalBills.isEmpty())
          Error = "Please Enter All Field";
        else
          Error = "Please Enter Values more than 0";
        validData = false;
      });
    }
    /***************if choose maxload ongrid but input not valid*********/
    else if (dropdownValue == "Max usage" && _isEnabled && maxloadvalueon <= 0) {
      setState(() {
        if (maxloadvalueon == -1)
          Error = "Please Enter Value";
        else
          Error = "Please Enter Value more than 0";
        validData = false;
      });
    } else if (_currentSliderValue == 0) {
      setState(() {
        validData = true;
      });
    }
    /**************if Select off Grid with invalid data****************/
    else if (!_isEnabled && maxloadvalueoff <= 0) {
      setState(() {
        if (maxloadvalueoff == -1)
          Error = "Please Enter Value";
        else
          Error = "Please Enter Value more than 0";
        validData = false;
      });
    }
    /***********Valid data and valid Electricity Coverage*********/
    else if (_currentSliderValue > 0) {
      setState(() {
        validData = true;
      });

      /***********Set data in inputdata***********/
      inpudata.elecitycoverage = _currentSliderValue;
      inpudata.maxusageoff = maxloadvalueoff;
      inpudata.maxusageon = maxloadvalueon;
      inpudata.ongrid = _isEnabled;
      if (_isEnabled && dropdownValue == "Max usage") {
        inpudata.maxOrbills = "Max";
      } else {
        inpudata.maxOrbills = "bills";
      }
Navigator.of(context)
          .push(MaterialPageRoute(builder: (_) =>Maxload()));
    }
  }
}

/*******************widget for header part**************************/
class Header extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: hig(100),
      width: double.infinity,
      child: Stack(
        children: [
          Positioned(
            top: 0,
            right: 0,
            child: Image.asset(
              'assets/inputcalculation/cloud.png',
              fit: BoxFit.fill,
              width: wid(175),
              height: hig(90),
            ),
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              GestureDetector(
                onTap: () {
                  Navigator.pushReplacement(
                    context,
                    MaterialPageRoute(builder: (context) => HomeUser(userr: user,)),
                  );

                  print('Arrow tapped');
                },
                child: Image.asset(
                  'assets/inputcalculation/arrow.png',
                  width: wid(87),
                  height: hig(80),
                ),
              ),
              Text(
                'Calculations',
                style: TextStyle(
                  fontSize: 25.0 * t,
                  fontWeight: FontWeight.bold,
                  color: Color(0xFF063221),
                ),
              ),
              SizedBox(width: wid(87)), // Spacer to balance layout
            ],
          ),
        ],
      ),
    );
  }
}

/*********************widget for swich buttom*************************/
class GridSwitch extends StatelessWidget {
  final bool isEnabled;
  final ValueChanged<bool> onChanged;

  GridSwitch({required this.isEnabled, required this.onChanged});

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Widget>[
        /********on or off********/
        Text(
          !isEnabled ? 'Off Grid' : 'ON Grid',
          style: TextStyle(
            fontSize: 17.0,
            fontWeight: FontWeight.bold,
            color: Color(0xFF063221),
          ),
        ),
        SizedBox(width: wid(12.5)),
        /*********Switch************/
        Switch(
          value: isEnabled,
          onChanged: (value) => onChanged(value),
          activeColor: Color(0xffFB9534), // Active color
          inactiveThumbColor: Colors.grey, // Inactive thumb color
          activeTrackColor: Color(0xFFECAB6D), // Active track color
          inactiveTrackColor: Color(0xFFC3C3C3), // Inactive track color
        ),
      ],
    );
  }
}

/*************widget for dropdown list ("Electricity" ot "Maxload")***********/
class DropdownSection extends StatelessWidget {
  final String dropdownValue;
  final bool isEnabled;
  final ValueChanged<String?> onChanged;

  DropdownSection({
    required this.dropdownValue,
    required this.isEnabled,
    required this.onChanged,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        SizedBox(
          height: hig(20),
          width: wid(155),
          child: !isEnabled
              ? Center(
                  child: Text(
                    "Max usage",
                    style: TextStyle(
                      color: Color(0xff4A4A4A),
                      fontWeight: FontWeight.bold,
                      fontSize: 15.0 * t,
                    ),
                  ),
                )
              : DropdownButtonHideUnderline(
                  child: DropdownButton<String>(
                    focusColor: Colors.white,
                    dropdownColor: Colors.white,
                    elevation: 0,
                    value: dropdownValue,
                    icon: Icon(
                      Icons.arrow_drop_down,
                      color: Colors.orange,
                    ),
                    onChanged: onChanged,
                    items: <String>['Electricity Usage', 'Max usage']
                        .map<DropdownMenuItem<String>>((String value) {
                      return DropdownMenuItem<String>(
                        value: value,
                        child: Text(
                          value,
                          style: TextStyle(
                            fontSize: 15.0 * t,
                            fontWeight: FontWeight.bold,
                            color: Color(0xFF063221),
                          ),
                        ),
                      );
                    }).toList(),
                  ),
                ),
        ),
        SizedBox(height: hig(4)),
        Container(height: hig(2), width: wid(125), color: Colors.grey)
      ],
    );
  }
}

/**************apper textfield after selection from dropdwon list************/
class MonthSection extends StatelessWidget {
  final String dropdownValue;
  final bool isEnabled;

  MonthSection({required this.dropdownValue, required this.isEnabled});

  @override
  Widget build(BuildContext context) {
    if (!isEnabled) {
      return Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          /*********************Off Grid******************/
          SizedBox(height: hig(10)),
          MonthContainer(
            month: 'Max usage',
            onChanged: (value) {
              maxloadvalueoff = double.tryParse(value) ?? -1;
              print(maxloadvalueoff);
            },
            initialValue: maxloadvalueoff == -1 ? -1 : maxloadvalueoff,
          ),
        ],
      );
    }
    /*****************on Grid and Electricity Bills****************/
    else if (dropdownValue == 'Electricity Usage') {
      return Column(
        children: [
          SizedBox(height: hig(10)),
          for (var months in [
            ['January', 'February'],
            ['March', 'April'],
            ['May', 'June'],
            ['July', 'August'],
            ['September', 'October'],
            ['November', 'December']
          ]) ...[
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: months
                  .map((month) => MonthContainer(
                        month: month,
                        onChanged: (value) => inpudata.electricalBills
                            .months[month] = num.tryParse(value) ?? -1,
                        initialValue: inpudata.electricalBills.months[month]!,
                      ))
                  .toList(),
            ),
            SizedBox(height: hig(10)),
          ],
        ],
      );
    }
    /***************On Grid and Max usage***************/
    else if (dropdownValue == 'Max usage') {
      return Column(
        children: [
          SizedBox(height: hig(10)),
          MonthContainer(
            month: 'Max usage',
            onChanged: (value) => maxloadvalueon = double.tryParse(value) ?? -1,
            initialValue: maxloadvalueon == -1 ? -1 : maxloadvalueon,
          ),
        ],
      );
    }
    return Container();
  }
}

/*********************widget for slider*****************************/
class ElectricalCoverageSlider extends StatelessWidget {
  final bool isEnabled;
  final double currentSliderValue;
  final ValueChanged<double> onChanged;
  final String dropdownValue;
  ElectricalCoverageSlider(
      {required this.isEnabled,
      required this.currentSliderValue,
      required this.onChanged,
      required this.dropdownValue});

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        Column(
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            SizedBox(
              height: hig(20),
              width: wid(174),
              child: Center(
                child: Text(
                  "Electrical Coverage",
                  style: TextStyle(
                    fontSize: 15.0 * t,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFF063221),
                  ),
                ),
              ),
            ),
            SizedBox(height: hig(4)),
            Container(height: hig(2), width: wid(125.5), color: Colors.grey)
          ],
        ),
        Container(
          width: wid(311.85),
          height: hig(30),
          child: SliderTheme(
            data: SliderTheme.of(context).copyWith(
              activeTrackColor: Color(0xffFB9534),
              inactiveTrackColor: Colors.grey,
              thumbColor: Color(0xffFB9534),
              overlayColor: Colors.orange.withAlpha(32),
              thumbShape: RoundSliderThumbShape(enabledThumbRadius: 10.0),
              overlayShape: RoundSliderOverlayShape(overlayRadius: 20.0),
              trackHeight: 2,
            ),
            child: Slider(
              value: !isEnabled ? 100 : currentSliderValue,
              min: 0,
              max: 100,
              onChanged: !isEnabled ? null : onChanged,
            ),
          ),
        ),
        SizedBox(height: hig(5)),
        Text(
          !isEnabled ? '100%' : '${currentSliderValue.toInt()}%',
          style: TextStyle(
            fontSize: 10.0 * t,
            fontWeight: FontWeight.bold,
            color: Color(0xFF063221),
          ),
        ),
        // if (dropdownValue != 'Max Load') SizedBox(height: hig(10)),
      ],
    );
  }
}

/***************widget for next button*******************/
class NextButton extends StatelessWidget {
  final void Function() onpress;
  NextButton({required this.onpress});
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.only(right: wid(12.5)),
      child: Align(
        alignment: Alignment.bottomRight,
        child: ElevatedButton(
          onPressed: onpress,
          style: ElevatedButton.styleFrom(
            foregroundColor: Colors.white,
            backgroundColor: Color(0xffFB9534),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(30.0),
            ),
            minimumSize: Size(wid(150), hig(40)),
          ),
          child: Text(
            'Next >',
            style: TextStyle(
              fontSize: 10.0 * t,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
        ),
      ),
    );
  }
}

/*****************widget for one text field container that apper**************************/
class MonthContainer extends StatefulWidget {
  final String month;
  final Function(String) onChanged;
  final num initialValue;
  MonthContainer({
    required this.month,
    required this.onChanged,
    required this.initialValue,
  });
  @override
  State<MonthContainer> createState() => _MonthContainerState();
}

class _MonthContainerState extends State<MonthContainer> {
  late TextEditingController _controller;
  @override
  void initState() {
    super.initState();
    _controller = TextEditingController(
      text: widget.initialValue != -1 ? widget.initialValue.toString() : '',
    );
    _controller.addListener(() {
      widget.onChanged(_controller.text);
    });
  }

  @override
  void didUpdateWidget(MonthContainer oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (oldWidget.initialValue != widget.initialValue) {
      _controller.text =
          widget.initialValue != -1 ? widget.initialValue.toString() : '';
    }
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    var w = MediaQuery.of(context).size.width;
    var h = MediaQuery.of(context).size.height;
    var t = MediaQuery.of(context).textScaleFactor;
    return Container(
      width: w * 0.323,
      height: h * 0.051,
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(50),
        color: Color(0xffD9EDCA),
      ),
      child: Center(
        child: TextField(
          obscureText: false,
          controller: _controller,
          keyboardType: TextInputType.numberWithOptions(decimal: true),
          inputFormatters: [
            FilteringTextInputFormatter.allow(RegExp(r'^(\d\d*\.|\d)\d*')),
          ],
          textAlign: TextAlign.center,
          decoration: InputDecoration(
            
            contentPadding: EdgeInsets.only(bottom: 0.02*h),
            border: InputBorder.none,
            hintText: widget.month,
            hintStyle: TextStyle(
              
              color: Color(0xff4A4A4A),
              fontWeight: FontWeight.bold,
              fontSize: 10.0 * t,
            ),
          ),
          
        ),
      ),
    );
  }
}
