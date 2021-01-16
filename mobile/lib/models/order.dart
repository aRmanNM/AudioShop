class Order{

  int id;

  Order({this.id});

  Order.fromJson(Map<String, dynamic> json)
      : id = json['id'];
}