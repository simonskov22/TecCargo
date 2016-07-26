<?php
include '../../../config.php';

$mode = $_POST['mode'];
$type = $_POST['type'];

$price = include 'kurerTransportRush-Flex.php';

echo $price;