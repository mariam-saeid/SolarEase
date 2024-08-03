import 'package:app/MarketPlace/APIService/SmallPostModel.dart';
import 'package:app/MarketPlace/PostView/CardWidet.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';



// ignore: must_be_immutable
class PostCard extends StatelessWidget {
  Smallpostmodel smallpostmodel;
  PostCard({required this.smallpostmodel});
  

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal:4,vertical: 2),
      child: CardWidet(smallpostmodel: smallpostmodel,),
    );
  }
}


