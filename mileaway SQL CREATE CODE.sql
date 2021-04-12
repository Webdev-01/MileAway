-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Gegenereerd op: 12 apr 2021 om 11:35
-- Serverversie: 10.4.11-MariaDB
-- PHP-versie: 7.4.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `mileaway`
--

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `costs`
--

CREATE TABLE `costs` (
  `Cost_ID` int(11) NOT NULL,
  `TypeCost_ID` int(11) NOT NULL,
  `License` varchar(255) NOT NULL,
  `Cost` double DEFAULT NULL,
  `Fuel_Quantity` double DEFAULT NULL,
  `Date_Of_Cost` date NOT NULL,
  `Invoice_Doc` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `costs`
--

INSERT INTO `costs` (`Cost_ID`, `TypeCost_ID`, `License`, `Cost`, `Fuel_Quantity`, `Date_Of_Cost`, `Invoice_Doc`) VALUES
(1, 3, 'AD-32-CD', 120.4, NULL, '2021-03-23', NULL),
(2, 4, 'AD-32-CD', 140.32, NULL, '2021-03-24', NULL),
(3, 4, 'PE-NM-23', 78.02, NULL, '2021-03-26', NULL),
(4, 3, 'PE-NM-23', 92.3, NULL, '2021-03-25', NULL);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `typecosts`
--

CREATE TABLE `typecosts` (
  `TypeCost_ID` int(11) NOT NULL,
  `TypeCost_Name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `typecosts`
--

INSERT INTO `typecosts` (`TypeCost_ID`, `TypeCost_Name`) VALUES
(1, 'Brandstof'),
(2, 'Reparatie'),
(3, 'Wegenbelasting'),
(4, 'Verzekering');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `users`
--

CREATE TABLE `users` (
  `Email` varchar(255) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `First_Name` varchar(255) NOT NULL,
  `Last_Name` varchar(255) NOT NULL,
  `User_Image` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `users`
--

INSERT INTO `users` (`Email`, `Password`, `First_Name`, `Last_Name`, `User_Image`) VALUES
('admin@admin', '$MYHASH$V1$10000$vjpftytHozuXQd19onJdmu40BI+8XQgfhmMUCTKwoS+2cLsI', 'Test', 'Test', 'avatar.png'),
('jesselootsma13@gmail.com', '$MYHASH$V1$10000$xPZp3ZKDhWQrOm8V4hLRoqkhzoLVq1AJs9h845uC3l5/xpwd', 'J', 'Lootsma', NULL);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `vehicles`
--

CREATE TABLE `vehicles` (
  `License` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Brand_Name` varchar(255) NOT NULL,
  `Model_Name` varchar(255) NOT NULL,
  `Manufacturing_Year` int(11) NOT NULL,
  `FuelType` varchar(255) DEFAULT NULL,
  `Color` varchar(255) NOT NULL,
  `Mileage_KM` int(11) NOT NULL,
  `Vehicle_Image` text DEFAULT NULL,
  `Insurance` double DEFAULT NULL,
  `Insurance_Date` date DEFAULT NULL,
  `Road_Tax` double DEFAULT NULL,
  `Road_Tax_Date` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `vehicles`
--

INSERT INTO `vehicles` (`License`, `Email`, `Brand_Name`, `Model_Name`, `Manufacturing_Year`, `FuelType`, `Color`, `Mileage_KM`, `Vehicle_Image`, `Insurance`, `Insurance_Date`, `Road_Tax`, `Road_Tax_Date`) VALUES
('37-DF-BG', 'admin@admin', 'Volkswagen', 'Golf', 1999, 'Benzine', 'Grijs', 255000, '170648966_461751848583143_397387541876560885_n.jpg', 60, '2021-04-11', 30, '2021-04-11'),
('AD-32-CD', 'admin@admin', 'VW', 'Golf', 2013, NULL, 'Grijs', 123000, NULL, 0, NULL, 0, NULL),
('PE-NM-23', 'admin@admin', 'Ford', 'Focus', 2003, NULL, 'Rood', 305056, NULL, 0, NULL, 0, NULL);

--
-- Indexen voor geëxporteerde tabellen
--

--
-- Indexen voor tabel `costs`
--
ALTER TABLE `costs`
  ADD PRIMARY KEY (`Cost_ID`),
  ADD KEY `License` (`License`),
  ADD KEY `TypeCost_ID` (`TypeCost_ID`);

--
-- Indexen voor tabel `typecosts`
--
ALTER TABLE `typecosts`
  ADD PRIMARY KEY (`TypeCost_ID`);

--
-- Indexen voor tabel `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Email`);

--
-- Indexen voor tabel `vehicles`
--
ALTER TABLE `vehicles`
  ADD PRIMARY KEY (`License`),
  ADD KEY `Email` (`Email`);

--
-- AUTO_INCREMENT voor geëxporteerde tabellen
--

--
-- AUTO_INCREMENT voor een tabel `costs`
--
ALTER TABLE `costs`
  MODIFY `Cost_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT voor een tabel `typecosts`
--
ALTER TABLE `typecosts`
  MODIFY `TypeCost_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Beperkingen voor geëxporteerde tabellen
--

--
-- Beperkingen voor tabel `costs`
--
ALTER TABLE `costs`
  ADD CONSTRAINT `costs_ibfk_1` FOREIGN KEY (`License`) REFERENCES `vehicles` (`License`),
  ADD CONSTRAINT `costs_ibfk_2` FOREIGN KEY (`TypeCost_ID`) REFERENCES `typecosts` (`TypeCost_ID`),


--
-- Beperkingen voor tabel `vehicles`
--
ALTER TABLE `vehicles`
  ADD CONSTRAINT `vehicles_ibfk_1` FOREIGN KEY (`Email`) REFERENCES `users` (`Email`),
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
