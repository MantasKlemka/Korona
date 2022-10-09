-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 09, 2022 at 10:01 PM
-- Server version: 10.4.25-MariaDB
-- PHP Version: 7.4.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `corona_base`
--

-- --------------------------------------------------------

--
-- Table structure for table `doctors`
--

CREATE TABLE `doctors` (
  `personal_code` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `activated` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `doctors`
--

INSERT INTO `doctors` (`personal_code`, `name`, `surname`, `password`, `activated`) VALUES
('38303155124', 'Gytis', 'Stankevicius', '123456789', 1),
('39202111412', 'Povilas', 'Petrasiunas', 'poviliux1234', 1),
('47309105167', 'Birute', 'Stankeviciene', 'rudis51231', 1),
('48611024125', 'Rita', 'Trasiunaite', 'trasiun1234', 1),
('49608148712', 'Ruta', 'Rasimaviciene', 'rasiruta1234', 1);

-- --------------------------------------------------------

--
-- Table structure for table `isolations`
--

CREATE TABLE `isolations` (
  `id` varchar(255) NOT NULL,
  `cause` varchar(255) NOT NULL,
  `start_date` date NOT NULL,
  `amount_of_days` int(11) NOT NULL,
  `pacient` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `isolations`
--

INSERT INTO `isolations` (`id`, `cause`, `start_date`, `amount_of_days`, `pacient`) VALUES
('421', 'Diagnosed with COVID-19', '2022-10-06', 15, '50001015553'),
('422', 'Been in contact with COVID-19', '2022-10-03', 14, '39912154553'),
('423', 'Has symptoms related to COVID-19', '2022-10-09', 7, '50602021321'),
('426', 'Has symptoms related to COVID-19', '2022-10-05', 14, '50305021111'),
('427', 'Came from hotspot country', '2022-10-08', 14, '49511024111');

-- --------------------------------------------------------

--
-- Table structure for table `pacients`
--

CREATE TABLE `pacients` (
  `personal_code` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NOT NULL,
  `birthdate` date NOT NULL,
  `phone_number` varchar(255) NOT NULL,
  `address` varchar(255) NOT NULL,
  `doctor` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `pacients`
--

INSERT INTO `pacients` (`personal_code`, `name`, `surname`, `birthdate`, `phone_number`, `address`, `doctor`) VALUES
('39912154553', 'Tomas', 'Tomasiunas', '1992-12-15', '+37062485839', 'Ilgenu g. 11, Vilnius', '39202111412'),
('49511024111', 'Arune', 'Berniene', '1995-11-02', '+37062248559', 'Pilies g. 44, Klaipeda', '48611024125'),
('50001015553', 'Jonas', 'Jonaitis', '2000-01-01', '+37066666666', 'Smilgų g. 50, Kaunas', '38303155124'),
('50305021111', 'Tomas', 'Bernelis', '2003-05-02', '+37066448123', 'Kudirkos g. 44, Ukmerge', '49608148712'),
('50602021321', 'Petras', 'Petrelis', '2006-02-02', '+37067544223', 'Liepu g. 11, Birzai', '47309105167');

-- --------------------------------------------------------

--
-- Table structure for table `tests`
--

CREATE TABLE `tests` (
  `id` varchar(255) NOT NULL,
  `date` date NOT NULL,
  `type` varchar(255) NOT NULL,
  `result` varchar(255) NOT NULL,
  `isolation` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tests`
--

INSERT INTO `tests` (`id`, `date`, `type`, `result`, `isolation`) VALUES
('224', '2022-10-08', 'PCR', 'POSITIVE', '421'),
('225', '2022-10-09', 'SELF TEST', 'NEGATIVE', '421'),
('226', '2022-10-09', 'ANTIGEN', 'POSITIVE', '421'),
('424', '2022-10-09', 'PCR', 'Negative', '422'),
('425', '2022-10-09', 'ANTIGEN', 'POSITIVE', '423');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `doctors`
--
ALTER TABLE `doctors`
  ADD PRIMARY KEY (`personal_code`);

--
-- Indexes for table `isolations`
--
ALTER TABLE `isolations`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_Pacient` (`pacient`);

--
-- Indexes for table `pacients`
--
ALTER TABLE `pacients`
  ADD PRIMARY KEY (`personal_code`),
  ADD KEY `FK_Doctor` (`doctor`);

--
-- Indexes for table `tests`
--
ALTER TABLE `tests`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_Isolation` (`isolation`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `isolations`
--
ALTER TABLE `isolations`
  ADD CONSTRAINT `FK_Pacient` FOREIGN KEY (`pacient`) REFERENCES `pacients` (`personal_code`);

--
-- Constraints for table `pacients`
--
ALTER TABLE `pacients`
  ADD CONSTRAINT `FK_Doctor` FOREIGN KEY (`doctor`) REFERENCES `doctors` (`personal_code`);

--
-- Constraints for table `tests`
--
ALTER TABLE `tests`
  ADD CONSTRAINT `FK_Isolation` FOREIGN KEY (`isolation`) REFERENCES `isolations` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
