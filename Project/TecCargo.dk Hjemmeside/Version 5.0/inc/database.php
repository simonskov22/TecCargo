<?php

$database = new DatabaseSetup();

class emptyObject {

     public function __construct(array $arguments = array()) {
        
        if (!empty($arguments)) {
            foreach ($arguments as $property => $argument) {
                $this->{$property} = $argument;
            }
        }    
    }

}
define("object", 0);
define("array_N", 1);
define("array_I", 2);


class DatabaseSetup {
    
    private $host = db_host;
    private $username = db_user;
    private $password = db_pass;
    private $database = db_database;
    public $prefix = db_prefix;
    public $lastError = "";
    public $lastQuery = "";
    private $fieldType = array(
        1 => "int",
        2 => "int",
        3 => "int",
        4 => "string",//"float_",
        5 => "string",//"double_",
        5 => "string",//"real_",
        7 => "string",//"timestamp_",
        8 => "int",
        9 => "int",
        10 => "string",//"date_",
        11 => "string",//"time_",
        12 => "string",//"datetime_",
        13 => "string",//"year_",
        16 => "string",//"bit_",
        246 => "string", //"decimal_",
        252 => "string", //"text_",
        253 => "string", //"varchar_",
        254 => "string" //"binary_"
        );




    private function SetupDatabase() {
        
        
    }
    
    private function ConvertValueType($value, $type) {
        
        if(key_exists($type, $this->fieldType)){
            if($this->fieldType[$type] === "int"){
                return (int) $value;
            }
        }
        
        return (string) $value;
    }
    
    public function ValueIsUnique($table,$value,$column) {
        $sqlConnect = new mysqli($this->host, $this->username, $this->password, $this->database);
        if (mysqli_connect_errno()) {
            echo "Connect failed: ". mysqli_connect_error();
            return false;
        }
        
        $sqlConnect->set_charset('utf8');
        $tableReady = $sqlConnect->real_escape_string($table);
        $valueReady = $sqlConnect->real_escape_string($value);
        $columnReady = $sqlConnect->real_escape_string($column);
        
        $query = "SELECT COUNT(*) FROM `$tableReady` WHERE `$columnReady` = '$valueReady';";
        $result = $sqlConnect->query($query);
        $this->lastQuery = $query;
        
        if(!$result){
            
            $this->lastError = "MySQL ERRNO: " .$sqlConnect->errno
                    ."\nMySQL Message: ". $sqlConnect->error;
            
            $sqlConnect->close();
            return false;
        }
        
        $rowCount = $result->fetch_assoc();
        
        $sqlConnect->close();
        
        if($rowCount['COUNT(*)'] == 0){
            return true;
        }
        else{
            return false;
        }
    }
    
    public function ValueIsSame($table,$value,$column,$where) {
        $sqlConnect = new mysqli($this->host, $this->username, $this->password, $this->database);
        if (mysqli_connect_errno()) {
            echo "Connect failed: ". mysqli_connect_error();
            return false;
        }
        
        $sqlConnect->set_charset('utf8');
        $tableReady = $sqlConnect->real_escape_string($table);
        $valueReady = $sqlConnect->real_escape_string($value);
        $columnReady = $sqlConnect->real_escape_string($column);
        
        $whereSql = "";
        
        foreach ($where as $keyColumn => $whereValue) {

            $keyColumn = $sqlConnect->real_escape_string($keyColumn);

            if(is_string($whereValue)){
                $whereValue = $sqlConnect->real_escape_string($whereValue);
                $sqlWhereValue = "'$whereValue' AND";
            }
            else{
                $sqlWhereValue = "$whereValue AND";
            }

            $whereSql .= "`$keyColumn` = $sqlWhereValue";
        }

        $whereSql = substr($whereSql, 0, strlen($whereSql)-3);

        $query = "SELECT COUNT(*) FROM `$tableReady` WHERE `$columnReady` = '$valueReady' AND $whereSql;";
        $result = $sqlConnect->query($query);
        $this->lastQuery = $query;
        
        if(!$result){
            
            $this->lastError = "MySQL ERRNO: " .$sqlConnect->errno
                    ."\nMySQL Message: ". $sqlConnect->error;
            
            $sqlConnect->close();
            return false;
        }
        
        $rowCount = $result->fetch_assoc();
        
        $sqlConnect->close();
        
        if($rowCount['COUNT(*)'] == 1){
            return true;
        }
        else{
            return false;
        }
    }
    
    public function Insert($table, $values) {
        $sqlConnect = new mysqli($this->host, $this->username, $this->password, $this->database);
        
        
        if (mysqli_connect_errno()) {
            echo "Connect failed: ". mysqli_connect_error();
            return false;
        }
        
        $sqlConnect->set_charset('utf8');
        $table = $sqlConnect->real_escape_string($table);
        
        $sqlColumn = "";
        $sqlValue = "";
        
        foreach ($values as $column => $value) {
            
            $column = $sqlConnect->real_escape_string($column);
            
            $sqlColumn .= "`$column`, ";
            
            if(is_string($value)){
                $value = $sqlConnect->real_escape_string($value);
                $sqlValue .= "'$value', ";
            }
            else{
                $sqlValue .= "$value, ";
            }
            
        }
        
        $sqlColumn = substr($sqlColumn, 0, strlen($sqlColumn)-2);
        $sqlValue = substr($sqlValue, 0, strlen($sqlValue)-2);
        
        $query = "INSERT INTO $table ($sqlColumn) VALUES($sqlValue);";
        $result = $sqlConnect->query($query);
        
        
        $this->lastQuery = $query;
        
        if(!$result){
            $this->lastError = "MySQL ERRNO: " .$sqlConnect->errno
                    ."\nMySQL Message: ". $sqlConnect->error;
            $sqlConnect->close();
            return false;
        }
        $sqlConnect->close();
        return true;
    }
    
    public function Delete($table,$where) {
        $sqlConnect = new mysqli($this->host, $this->username, $this->password, $this->database);
        
        if (mysqli_connect_errno()) {
            echo "Connect failed: ". mysqli_connect_error();
            return false;
        }
        
        $tableReady = $sqlConnect->real_escape_string($table);
        
        $sqlWhere = "";
        
        foreach ($where as $column => $value) {
            
            $column = $sqlConnect->real_escape_string($column);
            
            
            
            if(is_string($value)){
                $value = $sqlConnect->real_escape_string($value);
                $sqlValue = "'$value' AND";
            }
            else{
                $sqlValue = "$value AND";
            }
            
            $sqlWhere .= "`$column` = $sqlValue";
        }
        
        $sqlWhereReady = substr($sqlWhere, 0, strlen($sqlWhere)-3);
        
        $query = "DELETE FROM `$tableReady` WHERE $sqlWhereReady;";
        
        $this->lastQuery = $query;
        
        $result = $sqlConnect->query($query);
        
        if(!$result){
            $this->lastError = "MySQL ERRNO: " .$sqlConnect->errno
                    ."\nMySQL Message: ". $sqlConnect->error;
            $sqlConnect->close();
            return false;
        }
        $sqlConnect->close();
        return true;
    }
    
    public function Update($table,$values,$where = null) {
        $sqlConnect = new mysqli($this->host, $this->username, $this->password, $this->database);
        
        if (mysqli_connect_errno()) {
            echo "Connect failed: ". mysqli_connect_error();
            return false;
        }
        
        $sqlConnect->set_charset('utf8');
        $tableReady = $sqlConnect->real_escape_string($table);
        
        $sqlUpdate = "SET ";
        
        foreach ($values as $column => $setValue) {
            
            $column = $sqlConnect->real_escape_string($column);
            
            if(is_string($setValue)){
                $setValue = $sqlConnect->real_escape_string($setValue);
                $sqlSetValue = "'$setValue'";
            }
            else{
                $sqlSetValue = $setValue;
            }
            
            $sqlUpdate .= "`$column` = $sqlSetValue, ";
        }
        
        $sqlUpdateReady = substr($sqlUpdate, 0, strlen($sqlUpdate)-2);
        
        $sqlWhereReady = "";
        
        if($where != null){
            
            $sqlWhere = " WHERE ";
            
            foreach ($where as $column => $whereValue) {

                $column = $sqlConnect->real_escape_string($column);

                if(is_string($whereValue)){
                    $whereValue = $sqlConnect->real_escape_string($whereValue);
                    $sqlWhereValue = "'$whereValue' AND";
                }
                else{
                    $sqlWhereValue = "$whereValue AND";
                }

                $sqlWhere .= "`$column` = $sqlWhereValue";
            }
        
            $sqlWhereReady = substr($sqlWhere, 0, strlen($sqlWhere)-3);
        }
        
        $query = "UPDATE `$tableReady` {$sqlUpdateReady}{$sqlWhereReady};";
        $result = $sqlConnect->query($query);
        $this->lastQuery = $query;
        
        if(!$result){
            $this->lastError = "MySQL ERRNO: " .$sqlConnect->errno
                    ."\nMySQL Message: ". $sqlConnect->error;
            
            $sqlConnect->close();
            return false;
        }
        $sqlConnect->close();
        return true;
    }
    
    public function Custom($string) {
        
        $sqlConnect = new mysqli($this->host, $this->username, $this->password, $this->database);
        
        if (mysqli_connect_errno()) {
            return "Connect failed: ". mysqli_connect_error();
        }
        
        $result = $sqlConnect->query($string);
        $sqlConnect->close();
        
        return $result;
    }
    
    
    public function GetResults($string, $type = 0) {
        $sqlConnect = new mysqli($this->host, $this->username, $this->password, $this->database);
        
        if (mysqli_connect_errno()) {
            echo "Connect failed: ". mysqli_connect_error();
            
            $this->lastError = "MySQL ERRNO: " .$sqlConnect->errno
                    ."\nMySQL Message: ". $sqlConnect->error;
            $this->lastQuery = $string;
            $sqlConnect->close();
            
            return false;
        }
        
        $sqlConnect->set_charset('utf8');
        $sqlResult = $sqlConnect->query($string);
        $this->lastQuery = $string;
        //$sqlConnect->close();
        if (!$sqlResult) {
            
            $this->lastError = "MySQL ERRNO: " .$sqlConnect->errno
                    ."\nMySQL Message: ". $sqlConnect->error;
            $sqlConnect->close();
            return FALSE;
        }
        
        if($sqlResult->num_rows === 0){
            return false;
        }
        
        $sqlColumnInfo = $sqlResult->fetch_fields();
        $sqlColumnNames = array();
        $sqlColumnType = array();
        
        foreach ($sqlColumnInfo as $info) {
            $sqlColumnNames[] = $info->name;
            $sqlColumnType[] = $info->type;
        }
        
        
        $returnValues = array();
        
        $columnCount = count($sqlColumnNames);
        while($row = $sqlResult->fetch_assoc()) {
            
            switch ($type) {
                case 0:
                    $newValue = new emptyObject();
                    for($i = 0; $i < $columnCount; $i++){
                        $newValue->$sqlColumnNames[$i] = $this->ConvertValueType($row[$sqlColumnNames[$i]], $sqlColumnType[$i]);
                    }
                    break;
                case 1:
                    $newValue = array();
                    for($i = 0; $i < $columnCount; $i++){
                        $newValue[$sqlColumnNames[$i]] = $this->ConvertValueType($row[$sqlColumnNames[$i]], $sqlColumnType[$i]);
                    }
                    break;
                case 2:
                    $newValue = array();
                    for($i = 0; $i < $columnCount; $i++){
                        $newValue[] = $this->ConvertValueType($row[$sqlColumnNames[$i]], $sqlColumnType[$i]);
                    }
                    break;
            }
            
            $returnValues[] = $newValue;
        }
        
        return $returnValues;   
    }
    public function GetRow($string, $type = 0) {
        
        
        $sqlConnect = new mysqli($this->host, $this->username, $this->password, $this->database);
        
        if (mysqli_connect_errno()) {
            echo "Connect failed: ". mysqli_connect_error();
            return false;
        }
        
        $sqlConnect->set_charset('utf8');
        $sqlResult = $sqlConnect->query($string);
        $this->lastQuery = $string;
        
        
        if (!$sqlResult) {
            
            $this->lastError = "MySQL ERRNO: " .$sqlConnect->errno
                    ."\nMySQL Message: ". $sqlConnect->error;
            
            return false;
        }
        
        $sqlConnect->close();
        
        if($sqlResult->num_rows === 0){
            return false;
        }
        
        $sqlColumnInfo = $sqlResult->fetch_fields();
        $sqlColumnNames = array();
        $sqlColumnType = array();
        
        foreach ($sqlColumnInfo as $info) {
            $sqlColumnNames[] = $info->name;
            $sqlColumnType[] = $info->type;
        }
        
        
        $columnCount = count($sqlColumnNames);
        $row = $sqlResult->fetch_assoc();
            
        switch ($type) {
            case 0:
                $newValue = new emptyObject();
                for($i = 0; $i < $columnCount; $i++){
                    $newValue->$sqlColumnNames[$i] = $this->ConvertValueType($row[$sqlColumnNames[$i]], $sqlColumnType[$i]);
                }
                break;
            case 1:
                for($i = 0; $i < $columnCount; $i++){
                    $newValue[$sqlColumnNames[$i]] = $this->ConvertValueType($row[$sqlColumnNames[$i]], $sqlColumnType[$i]);
                }
                break;
            case 2:
                for($i = 0; $i < $columnCount; $i++){
                    $newValue[] = $this->ConvertValueType($row[$sqlColumnNames[$i]], $sqlColumnType[$i]);
                }
                break;
        }

        $returnValues = $newValue;
        
        
        return $returnValues;   
    }
    
    public function SQLReadyString($string) {
        $sqlConnect = new mysqli($this->host, $this->username, $this->password, $this->database);
        $string = $sqlConnect->real_escape_string($string);
        $sqlConnect->close();
        
        return $string;
    }
   
}