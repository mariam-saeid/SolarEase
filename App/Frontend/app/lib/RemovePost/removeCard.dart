import 'package:app/RemovePost/APIForRemovePosts/removedpost.dart';
import 'package:app/editpost.dart';
import 'package:app/whenclick.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';

class RemoveCard extends StatelessWidget {
  final RemovedPost removedPost;
  Function(int id) onDelete;
  Function() update;

  RemoveCard(
      {required this.removedPost,
      required this.onDelete,
      required this.update});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 16),
      child: SizedBox(
        width: 130,
        child: Card(
          color: Color(0xffD9EDCA),
          child: Row(
            children: [
              // -------------------- Image -----------------
              Container(
                padding: EdgeInsets.only(left: 16),
                width: 60,
                height: 60,
                decoration:
                    BoxDecoration(borderRadius: BorderRadius.circular(32)),
                child: Image.network("http://solareasegp.runasp.net/" +
                    removedPost.RemovedImage),
              ),
              SizedBox(width: 32),
              // ------------------ Text ---------------------
              Padding(
                padding: const EdgeInsets.only(left: 16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Padding(
                      padding: const EdgeInsets.only(top: 8),
                      child: Text(
                        removedPost.RemovedCategory,
                        style: TextStyle(fontWeight: FontWeight.bold),
                      ),
                    ),
                    Text(removedPost.RemovedPrice),
                    Text(removedPost.RemovedDate),
                    Text(removedPost.RemovedLocation),
                    removedPost.PostStatus.toString() == "true"
                        ? Text("Approved",
                            style: TextStyle(
                                fontWeight: FontWeight.bold,
                                color: Colors.green))
                        : Text("Pending",
                            style: TextStyle(
                                fontWeight: FontWeight.bold,
                                color: Colors.orange)),
                  ],
                ),
              ),
              // ----------------------- Delete Icon ------
              SizedBox(width: 16),
              Padding(
                padding: const EdgeInsets.only(top: 16, left: 16),
                child: Column(
                  children: [
                    IconButton(
                      icon: Icon(Icons.delete, color: Colors.orange),
                      onPressed: () {
                        onDelete(removedPost.ID);
                      },
                    ),
                    Text("delete"),
                  ],
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 16),
                child: Column(
                  children: [
                    IconButton(
                      icon: Icon(Icons.edit),
                      onPressed: ()  {
                         Navigator.pushReplacement(
                          context,
                          MaterialPageRoute(builder: (context) => editPostApp(postid: removedPost.ID)),
                        );
                      
                      },
                    ),
                    Text("Edit"),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
