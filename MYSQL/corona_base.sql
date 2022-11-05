-- phpMyAdmin SQL Dump
-- version 5.1.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 05, 2022 at 10:33 AM
-- Server version: 10.4.24-MariaDB
-- PHP Version: 7.4.29

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
-- Table structure for table `administrators`
--

CREATE TABLE `administrators` (
  `id` int(11) NOT NULL,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `administrators`
--

INSERT INTO `administrators` (`id`, `username`, `password`) VALUES
(1, 'popas', '145632');

-- --------------------------------------------------------

--
-- Table structure for table `doctors`
--

CREATE TABLE `doctors` (
  `id` int(11) NOT NULL,
  `email` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `activated` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `doctors`
--

INSERT INTO `doctors` (`id`, `email`, `name`, `surname`, `password`, `activated`) VALUES
(150, 'gytux@gmail.com', 'Gytis', 'Stankevicius', '123456789', 1),
(151, 'povka@yahoo.com', 'Povilas', 'Petrasiunas', 'poviliux1234', 1),
(152, 'birutele@gmail.com', 'Birute', 'Stankeviciene', 'rudis51231', 1),
(153, 'trasiunaite222@gmail.com', 'Rita', 'Trasiunaite', 'trasiun1234', 1),
(154, 'rasimavruta@gmail.com', 'Ruta', 'Rasimaviciene', 'rasiruta1234', 1),
(155, 'gaa@gmail.com', 'Gytis', 'Stankevicius', '123456789', 1);

-- --------------------------------------------------------

--
-- Table structure for table `isolations`
--

CREATE TABLE `isolations` (
  `id` int(11) NOT NULL,
  `cause` varchar(255) NOT NULL,
  `start_date` date NOT NULL,
  `amount_of_days` int(11) NOT NULL,
  `pacient` int(11) NOT NULL,
  `code` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `isolations`
--

INSERT INTO `isolations` (`id`, `cause`, `start_date`, `amount_of_days`, `pacient`, `code`) VALUES
(9, 'Diagnosed with COVID-19', '2022-10-06', 7, 11, 'dadad'),
(10, 'Came from hotspot country', '2022-10-20', 7, 11, 'ggg'),
(11, 'Been in contact with COVID-20', '2022-10-03', 20, 12, 'g'),
(12, 'Has symptoms related to COVID-19', '2022-10-09', 7, 13, 'gg'),
(13, 'Has symptoms related to COVID-19', '2022-10-05', 14, 14, 'gggg'),
(14, 'Came from hotspot country', '2022-10-08', 14, 15, 'ggggggg'),
(15, 'Diagnosed with COVID-19', '2022-10-06', 4, 11, '1119514655');

-- --------------------------------------------------------

--
-- Table structure for table `pacients`
--

CREATE TABLE `pacients` (
  `id` int(11) NOT NULL,
  `identification_code` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NOT NULL,
  `birthdate` date NOT NULL,
  `phone_number` varchar(255) NOT NULL,
  `address` varchar(255) NOT NULL,
  `doctor` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `pacients`
--

INSERT INTO `pacients` (`id`, `identification_code`, `name`, `surname`, `birthdate`, `phone_number`, `address`, `doctor`) VALUES
(11, '5421215', 'Tomas', 'Tomasiunas', '1992-12-15', '+37062485839', 'Ilgenu g. 11, Vilnius', 150),
(12, '4847895', 'Arune', 'Berniene', '1995-11-02', '+37062248559', 'Pilies g. 44, Klaipeda', 151),
(13, '50001015553', 'Jonas', 'Jonaitis', '2000-01-01', '+37066666666', 'Smilgų g. 50, Kaunas', 152),
(14, '21515477', 'Tomas', 'Bernelis', '2003-05-02', '+37066448123', 'Kudirkos g. 44, Ukmerge', 152),
(15, '50602021321', 'Petras', 'Petrelis', '2006-02-02', '+37067544223', 'Liepu g. 11, Birzai', 154),
(16, '50602021322', 'Petras', 'Petrelis', '2006-02-02', '+37067544223', 'Liepu g. 11, Birzai', 153),
(22, '5000101555', 'Jonas', 'Jonaitis', '2000-01-01', '+37066666666', 'Smilgų g. 50, Kaunas', 154);

-- --------------------------------------------------------

--
-- Table structure for table `tests`
--

CREATE TABLE `tests` (
  `id` int(11) NOT NULL,
  `date` date NOT NULL,
  `type` varchar(255) NOT NULL,
  `result` varchar(255) NOT NULL,
  `isolation` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tests`
--

INSERT INTO `tests` (`id`, `date`, `type`, `result`, `isolation`) VALUES
(4, '2022-10-08', 'PCR', 'POSITIVE', 9),
(5, '2022-10-09', 'SELF TEST', 'NEGATIVE', 9),
(6, '2022-10-09', 'ANTIGEN', 'POSITIVE', 10),
(7, '2022-10-09', 'PCR', 'Negative', 10),
(8, '2022-10-09', 'ANTIGEN', 'POSITIVE', 10),
(9, '2022-10-08', 'PCR', 'POSITIVE', 9),
(21, '2022-10-08', 'PCR', 'POSITIVE', 9);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `administrators`
--
ALTER TABLE `administrators`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`username`);

--
-- Indexes for table `doctors`
--
ALTER TABLE `doctors`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indexes for table `isolations`
--
ALTER TABLE `isolations`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `code` (`code`),
  ADD KEY `FK_pacient` (`pacient`);

--
-- Indexes for table `pacients`
--
ALTER TABLE `pacients`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `identification_code` (`identification_code`),
  ADD KEY `FK_doctor` (`doctor`);

--
-- Indexes for table `tests`
--
ALTER TABLE `tests`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK_isolation` (`isolation`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `administrators`
--
ALTER TABLE `administrators`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `doctors`
--
ALTER TABLE `doctors`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=157;

--
-- AUTO_INCREMENT for table `isolations`
--
ALTER TABLE `isolations`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `pacients`
--
ALTER TABLE `pacients`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT for table `tests`
--
ALTER TABLE `tests`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `isolations`
--
ALTER TABLE `isolations`
  ADD CONSTRAINT `FK_pacient` FOREIGN KEY (`pacient`) REFERENCES `pacients` (`id`);

--
-- Constraints for table `pacients`
--
ALTER TABLE `pacients`
  ADD CONSTRAINT `FK_doctor` FOREIGN KEY (`doctor`) REFERENCES `doctors` (`id`);

--
-- Constraints for table `tests`
--
ALTER TABLE `tests`
  ADD CONSTRAINT `FK_isolation` FOREIGN KEY (`isolation`) REFERENCES `isolations` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
