-- DatabaseScripts/001_InitialCreate.sql
-- Vehicle Service Tracker Database Schema
-- PostgreSQL

-- Create Users Table
CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(500) NOT NULL,
    full_name VARCHAR(200),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP
);

-- Create Service Entries Table
CREATE TABLE IF NOT EXISTS service_entries (
    id UUID PRIMARY KEY,
    license_plate VARCHAR(20) NOT NULL,
    brand_name VARCHAR(100) NOT NULL,
    model_name VARCHAR(100) NOT NULL,
    kilometers INTEGER NOT NULL CHECK (kilometers >= 0),
    model_year INTEGER,
    service_date TIMESTAMP NOT NULL,
    has_warranty BOOLEAN,
    service_city VARCHAR(100),
    service_note VARCHAR(1000),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP
);

-- Create Indexes
CREATE INDEX IF NOT EXISTS idx_service_entries_license_plate ON service_entries(license_plate);
CREATE INDEX IF NOT EXISTS idx_service_entries_service_date ON service_entries(service_date DESC);
CREATE INDEX IF NOT EXISTS idx_service_entries_created_at ON service_entries(created_at DESC);
CREATE INDEX IF NOT EXISTS idx_users_username ON users(username);