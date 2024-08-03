import 'package:app/favorite/APIService/PostModel.dart';
import 'package:app/favorite/APIService/favoriteModel.dart';
import 'package:app/favorite/PostView/favoriteCard.dart';
import 'package:app/whenclick.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';

class FavoritBody extends StatefulWidget {
  @override
  _FavoritBodyState createState() => _FavoritBodyState();
}

class _FavoritBodyState extends State<FavoritBody> {
  List<FavoritePostmodel> _allPostsFuture = [];
  FavoriteService _favoriteService = FavoriteService(Dio());
  bool FavoritPost = false;
  @override
  void initState() {
    super.initState();
    fun();
  }

  Future<void> fun() async {
    setState(() {
      FavoritPost = true;
    });
    _allPostsFuture = await _favoriteService.getFavoritePosts(_allPostsFuture);
    setState(() {
      FavoritPost = false;
    });

    print(_allPostsFuture);
    setState(() {});
  }

  // Function to handle deletion of favorite post
  Future<void> onDelete() async {
    // await UserPostService(Dio()).ToggleFavorite(post.ID);
    setState(() {
      FavoritPost = true;
    });
    _allPostsFuture = await _favoriteService.getFavoritePosts(_allPostsFuture);
    setState(() {});
    setState(() {
      FavoritPost = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: [
          ListView.builder(
            itemCount: _allPostsFuture.length,
            itemBuilder: (context, index) {
              return FavoritePostCard(
                favoritepost: _allPostsFuture[index],
                onDelete: onDelete,
                postId: _allPostsFuture[index].ID,
                // Pass onDelete callback
              );
            },
          ),
          if (FavoritPost)
            const Opacity(
                opacity: 0,
                child: ModalBarrier(dismissible: false, color: Colors.black)),
          if (FavoritPost)
            const Center(
              child: CircularProgressIndicator(),
            )
        ],
      ),
    );
  }
}
