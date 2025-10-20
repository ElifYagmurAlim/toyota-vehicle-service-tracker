-- DatabaseScripts/003_SampleData.sql
-- Sample Service Entries for Testing (Optional)

INSERT INTO service_entries (
    id, 
    license_plate, 
    brand_name, 
    model_name, 
    kilometers, 
    model_year, 
    service_date, 
    has_warranty, 
    service_city, 
    service_note,
    created_at
) VALUES
(
    gen_random_uuid(),
    '34ABC123',
    'Toyota',
    'Corolla',
    45000,
    2020,
    '2024-10-15 10:30:00',
    true,
    'İstanbul',
    'Rutin bakım yapıldı. Yağ değişimi ve filtre değişimi.',
    CURRENT_TIMESTAMP
),
(
    gen_random_uuid(),
    '06XYZ789',
    'Toyota',
    'RAV4',
    78000,
    2019,
    '2024-10-14 14:45:00',
    false,
    'Ankara',
    'Fren balatası değişimi yapıldı.',
    CURRENT_TIMESTAMP
),
(
    gen_random_uuid(),
    '35DEF456',
    'Toyota',
    'Yaris',
    32000,
    2021,
    '2024-10-13 09:15:00',
    true,
    'İzmir',
    'Klima gazı takviyesi ve AC kontrolü.',
    CURRENT_TIMESTAMP
),
(
    gen_random_uuid(),
    '16GHI789',
    'Toyota',
    'C-HR',
    55000,
    2020,
    '2024-10-12 16:20:00',
    true,
    'Bursa',
    'Periyodik bakım ve lastik rotasyonu.',
    CURRENT_TIMESTAMP
),
(
    gen_random_uuid(),
    '34JKL012',
    'Toyota',
    'Camry',
    120000,
    2018,
    '2024-10-11 11:00:00',
    false,
    'İstanbul',
    'Triger seti değişimi yapıldı.',
    CURRENT_TIMESTAMP
);