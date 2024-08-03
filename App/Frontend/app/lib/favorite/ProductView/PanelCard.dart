import 'package:app/favorite/APIService/ProductModel.dart';
import 'package:app/favorite/FavoritePage.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:app/favorite/APIService/ProductService.dart';

class PanelCardList extends StatefulWidget {
  @override
  _PanelCardListState createState() => _PanelCardListState();
}

class _PanelCardListState extends State<PanelCardList> {
  List<FavoritePanel> _favoritePanelsFuture = [];
  bool panelLoading = false;
  @override
  void initState() {
    super.initState();
    _refreshFavoritePanels();
  }

  Future<void> _refreshFavoritePanels() async {
    setState(() {
      panelLoading = true;
    });
    _favoritePanelsFuture = await PanelService(Dio()).getFavoritePanel();
    setState(() {
      panelLoading = false;
    });
    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: Stack(
        children: [
          ListView.builder(
            itemCount: (_favoritePanelsFuture.length / 2).ceil(),
            itemBuilder: (context, index) {
              int firstIndex = index * 2;
              int secondIndex = firstIndex + 1;
              bool hasSecond = secondIndex < _favoritePanelsFuture.length;

              return Padding(
                padding: const EdgeInsets.only(bottom: 8.0),
                child: Row(
                  children: [
                    Padding(
                      padding: const EdgeInsets.only(left: 8.0),
                      child: PanelCard(
                        panelModel: _favoritePanelsFuture[firstIndex],
                        onFavoriteToggled: _refreshFavoritePanels,
                      ),
                    ),
                    if (hasSecond)
                      Padding(
                        padding: const EdgeInsets.only(left: 8.0),
                        child: PanelCard(
                          panelModel: _favoritePanelsFuture[secondIndex],
                          onFavoriteToggled: _refreshFavoritePanels,
                        ),
                      ),
                  ],
                ),
              );
            },
          ),
          if (panelLoading)
            const Opacity(
                opacity: 0,
                child: ModalBarrier(dismissible: false, color: Colors.black)),
          if (panelLoading)
            const Center(
              child: CircularProgressIndicator(),
            )
        ],
      ),
    );
  }
}

class PanelCard extends StatefulWidget {
  final FavoritePanel panelModel;
  Future<void> Function() onFavoriteToggled;

  PanelCard({required this.panelModel, required this.onFavoriteToggled});
  @override
  _PanelCardState createState() => _PanelCardState();
}

class _PanelCardState extends State<PanelCard> {
  PanelService? panelService = PanelService(Dio()); //PanelService
  bool _isLoading = false;

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: width(370),
      height: height(140),
      child: Card(
        color: Color(0xffD9EDCA),
        child: Column(
          children: [
            Align(
              alignment: Alignment.centerLeft,
              child: _isLoading
                  ? CircularProgressIndicator()
                  : IconButton(
                      onPressed: () async {
                        setState(() {
                          _isLoading = true;
                        });
                        try {
                          await panelService?.ToggleFavorite(
                              widget.panelModel.PanelId);
                          setState(() {
                            widget.panelModel.isFavorite =
                                !widget.panelModel.isFavorite;
                            _isLoading = false;
                          });
                          await widget.onFavoriteToggled();
                        } catch (e) {
                          setState(() {
                            _isLoading = false;
                          });
                        }
                      },
                      icon: Icon(
                        Icons.favorite,
                        color: Colors.orange,
                      ),
                    ),
            ),
            Column(
              children: [
                Align(
                  alignment: Alignment.center,
                  child: _buildImage(panelModel: widget.panelModel),
                ),
                Padding(
                  padding: const EdgeInsets.only(left: 16),
                  child: _TextBuilder(panelModel: widget.panelModel),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

Widget _TextBuilder({required panelModel}) {
  return Column(
    crossAxisAlignment: CrossAxisAlignment.start,
    children: [
      Text(
        panelModel.PanelBrandName,
        style: TextStyle(fontSize: 15, fontWeight: FontWeight.bold),
      ),
      Row(
        children: [
          Icon(Icons.monetization_on, size: 16),
          Text(
            panelModel.PanelPrice,
            style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
          ),
        ],
      ),
      Row(
        children: [
          Icon(Icons.location_on, size: 16),
          Text(
            panelModel.totalPrice ?? 'N/A',
            style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
          ),
        ],
      ),
      Row(
        children: [
          Icon(Icons.calendar_today, size: 16),
          Text(
            panelModel.PanelCapacity,
            style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
          ),
        ],
      ),
    ],
  );
}

SizedBox _buildImage({required panelModel}) {
  return SizedBox(
    width: width(220),
    height: height(50),
    child:
        Image.network("http://solareasegp.runasp.net/" + panelModel.Panelimage),
  );
}
