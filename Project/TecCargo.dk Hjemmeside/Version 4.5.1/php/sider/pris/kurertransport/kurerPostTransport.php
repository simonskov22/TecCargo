<?php
    include_once '../../../config.php';
    getUserRole(true, false);

$mode = $_POST['mode'];
$type = $_POST['type'];

$price = include 'kurerTransportRush-Flex.php';

echo $price;