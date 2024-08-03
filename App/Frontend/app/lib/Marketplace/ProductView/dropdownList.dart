import 'package:app/MarketPlace/ProductView/BatteryCard.dart';
import 'package:app/MarketPlace/ProductView/InverterCard.dart';
import 'package:app/MarketPlace/ProductView/PanelCard.dart';
import 'package:app/MarketPlace/ProductView/Search.dart';
import 'package:app/addpost.dart';
import 'package:app/main.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class DropDownList extends StatefulWidget {
  const DropDownList({Key? key}) : super(key: key);

  @override
  State<DropDownList> createState() => _DropDownListState();
}

class _DropDownListState extends State<DropDownList> {
  String _selectedItem = 'Panel';

  @override
  Widget build(BuildContext context) {
    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Container(
          padding: EdgeInsets.only(left: 10),
          height: height(20),
          width: width(220),
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(12),
            // border: Border.all(color: Colors.orange),
          ),
          child: DropdownButton<String>(
            focusColor: Color(0xffFAFAFA),
            dropdownColor: Color(0xffFAFAFA),
            elevation: 0,
            icon: Icon(Icons.arrow_drop_down, color: Colors.orange),
            underline: Container(),
            style: TextStyle(color: Colors.black, fontWeight: FontWeight.bold),
            value: _selectedItem,
            items: <String>['Panel', 'Inverter', 'Battery'].map((String value) {
              return DropdownMenuItem<String>(
                value: value,
                child: Align(
                  alignment: Alignment.center,
                  child: Text(
                    value,
                    textAlign: TextAlign.center,
                    style: TextStyle(
                      fontSize: 15.0,
                      fontWeight: FontWeight.bold,
                      color: Color(0x0ff063221),
                    ),
                  ),
                ),
              );
            }).toList(),
            onChanged: (newValue) {
              setState(() {
                _selectedItem = newValue!;
              });
            },
          ),
        ),
        Container(
          width: width(250),
          height: height(3),
          color: Color(0x0ffBBBAB6),
        ),
        SizedBox(height: 8),
        // SearchWidgwt(onpress: (g) async {}),
        SizedBox(height: 8),
        _selectedItem == 'Panel'
            ? PanelCardList()
            : _selectedItem == 'Inverter'
                ? InverterCardList() //InverterCardList()
                : BatteryCardList(),
      ],
    );
  }
}
