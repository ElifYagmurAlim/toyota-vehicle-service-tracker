import { useState, FormEvent } from 'react';
import { useMutation } from '@tanstack/react-query';
import { serviceEntriesApi } from '../services/api';
import type { CreateServiceEntryDto } from '../types';

interface ServiceEntryModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

const turkishCities = [
  'Adana', 'Adıyaman', 'Afyonkarahisar', 'Ağrı', 'Aksaray', 'Amasya', 'Ankara', 'Antalya',
  'Ardahan', 'Artvin', 'Aydın', 'Balıkesir', 'Bartın', 'Batman', 'Bayburt', 'Bilecik',
  'Bingöl', 'Bitlis', 'Bolu', 'Burdur', 'Bursa', 'Çanakkale', 'Çankırı', 'Çorum',
  'Denizli', 'Diyarbakır', 'Düzce', 'Edirne', 'Elazığ', 'Erzincan', 'Erzurum', 'Eskişehir',
  'Gaziantep', 'Giresun', 'Gümüşhane', 'Hakkâri', 'Hatay', 'Iğdır', 'Isparta', 'İstanbul',
  'İzmir', 'Kahramanmaraş', 'Karabük', 'Karaman', 'Kars', 'Kastamonu', 'Kayseri', 'Kilis',
  'Kırıkkale', 'Kırklareli', 'Kırşehir', 'Kocaeli', 'Konya', 'Kütahya', 'Malatya', 'Manisa',
  'Mardin', 'Mersin', 'Muğla', 'Muş', 'Nevşehir', 'Niğde', 'Ordu', 'Osmaniye', 'Rize',
  'Sakarya', 'Samsun', 'Şanlıurfa', 'Siirt', 'Sinop', 'Sivas', 'Şırnak', 'Tekirdağ',
  'Tokat', 'Trabzon', 'Tunceli', 'Uşak', 'Van', 'Yalova', 'Yozgat', 'Zonguldak'
];

const ServiceEntryModal = ({ isOpen, onClose, onSuccess }: ServiceEntryModalProps) => {
  const [formData, setFormData] = useState<CreateServiceEntryDto>({
    licensePlate: '',
    brandName: '',
    modelName: '',
    kilometers: 0,
    modelYear: undefined,
    serviceDate: new Date().toISOString().split('T')[0],
    hasWarranty: undefined,
    serviceCity: '',
    serviceNote: '',
  });

  const [errors, setErrors] = useState<Record<string, string>>({});

  const mutation = useMutation({
    mutationFn: serviceEntriesApi.create,
    onSuccess: () => {
      resetForm();
      onSuccess();
    },
    onError: (error: any) => {
      const message = error.response?.data?.message || 'Bir hata oluştu';
      setErrors({ general: message });
    },
  });

  const resetForm = () => {
    setFormData({
      licensePlate: '',
      brandName: '',
      modelName: '',
      kilometers: 0,
      modelYear: undefined,
      serviceDate: new Date().toISOString().split('T')[0],
      hasWarranty: undefined,
      serviceCity: '',
      serviceNote: '',
    });
    setErrors({});
  };

  const validate = (): boolean => {
    const newErrors: Record<string, string> = {};

    if (!formData.licensePlate.trim()) {
      newErrors.licensePlate = 'Araç plakası zorunludur';
    }

    if (!formData.brandName.trim()) {
      newErrors.brandName = 'Marka adı zorunludur';
    }

    if (!formData.modelName.trim()) {
      newErrors.modelName = 'Model adı zorunludur';
    }

    if (formData.kilometers < 0) {
      newErrors.kilometers = 'KM bilgisi negatif olamaz';
    }

    if (!formData.serviceDate) {
      newErrors.serviceDate = 'Servis tarihi zorunludur';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();

    if (!validate()) return;

    const submitData: CreateServiceEntryDto = {
      ...formData,
      licensePlate: formData.licensePlate.trim().toUpperCase(),
      brandName: formData.brandName.trim(),
      modelName: formData.modelName.trim(),
      modelYear: formData.modelYear || undefined,
      serviceCity: formData.serviceCity?.trim() || undefined,
      serviceNote: formData.serviceNote?.trim() || undefined,
    };

    mutation.mutate(submitData);
  };

  const handleClose = () => {
    if (!mutation.isPending) {
      resetForm();
      onClose();
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-xl shadow-2xl w-full max-w-2xl max-h-[90vh] overflow-y-auto">
        {/* Header */}
        <div className="px-6 py-4 border-b border-gray-200 flex justify-between items-center sticky top-0 bg-white">
          <h3 className="text-xl font-semibold text-gray-900">Yeni Servis Girişi</h3>
          <button
            onClick={handleClose}
            disabled={mutation.isPending}
            className="text-gray-400 hover:text-gray-600 transition disabled:opacity-50"
          >
            <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>

        {/* Form */}
        <form onSubmit={handleSubmit} className="px-6 py-4 space-y-4">
          {errors.general && (
            <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
              {errors.general}
            </div>
          )}

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {/* License Plate */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Araç Plakası <span className="text-red-600">*</span>
              </label>
              <input
                type="text"
                value={formData.licensePlate}
                onChange={(e) => setFormData({ ...formData, licensePlate: e.target.value })}
                className={`w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-red-500 focus:border-transparent ${
                  errors.licensePlate ? 'border-red-500' : 'border-gray-300'
                }`}
                placeholder="34ABC123"
              />
              {errors.licensePlate && (
                <p className="mt-1 text-sm text-red-600">{errors.licensePlate}</p>
              )}
            </div>

            {/* Brand Name */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Marka Adı <span className="text-red-600">*</span>
              </label>
              <input
                type="text"
                value={formData.brandName}
                onChange={(e) => setFormData({ ...formData, brandName: e.target.value })}
                className={`w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-red-500 focus:border-transparent ${
                  errors.brandName ? 'border-red-500' : 'border-gray-300'
                }`}
                placeholder="Toyota"
              />
              {errors.brandName && (
                <p className="mt-1 text-sm text-red-600">{errors.brandName}</p>
              )}
            </div>

            {/* Model Name */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Model Adı <span className="text-red-600">*</span>
              </label>
              <input
                type="text"
                value={formData.modelName}
                onChange={(e) => setFormData({ ...formData, modelName: e.target.value })}
                className={`w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-red-500 focus:border-transparent ${
                  errors.modelName ? 'border-red-500' : 'border-gray-300'
                }`}
                placeholder="Corolla"
              />
              {errors.modelName && (
                <p className="mt-1 text-sm text-red-600">{errors.modelName}</p>
              )}
            </div>

            {/* Kilometers */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                KM Bilgisi <span className="text-red-600">*</span>
              </label>
              <input
                type="number"
                value={formData.kilometers}
                onChange={(e) => setFormData({ ...formData, kilometers: parseInt(e.target.value) || 0 })}
                className={`w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-red-500 focus:border-transparent ${
                  errors.kilometers ? 'border-red-500' : 'border-gray-300'
                }`}
                placeholder="45000"
                min="0"
              />
              {errors.kilometers && (
                <p className="mt-1 text-sm text-red-600">{errors.kilometers}</p>
              )}
            </div>

            {/* Model Year */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Model Yılı
              </label>
              <input
                type="number"
                value={formData.modelYear || ''}
                onChange={(e) => setFormData({ ...formData, modelYear: e.target.value ? parseInt(e.target.value) : undefined })}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-red-500 focus:border-transparent"
                placeholder="2020"
                min="1900"
                max={new Date().getFullYear() + 1}
              />
            </div>

            {/* Service Date */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Servise Geliş Tarihi <span className="text-red-600">*</span>
              </label>
              <input
                type="date"
                value={formData.serviceDate}
                onChange={(e) => setFormData({ ...formData, serviceDate: e.target.value })}
                className={`w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-red-500 focus:border-transparent ${
                  errors.serviceDate ? 'border-red-500' : 'border-gray-300'
                }`}
              />
              {errors.serviceDate && (
                <p className="mt-1 text-sm text-red-600">{errors.serviceDate}</p>
              )}
            </div>

            {/* Has Warranty */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Garantisi Var mı?
              </label>
              <select
                value={formData.hasWarranty === undefined ? '' : formData.hasWarranty.toString()}
                onChange={(e) =>
                  setFormData({
                    ...formData,
                    hasWarranty: e.target.value === '' ? undefined : e.target.value === 'true',
                  })
                }
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-red-500 focus:border-transparent"
              >
                <option value="">Seçiniz</option>
                <option value="true">Var</option>
                <option value="false">Yok</option>
              </select>
            </div>

            {/* Service City */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Servis Hizmeti Alınan Şehir
              </label>
              <select
                value={formData.serviceCity || ''}
                onChange={(e) => setFormData({ ...formData, serviceCity: e.target.value })}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-red-500 focus:border-transparent"
              >
                <option value="">Seçiniz</option>
                {turkishCities.map((city) => (
                  <option key={city} value={city}>
                    {city}
                  </option>
                ))}
              </select>
            </div>
          </div>

          {/* Service Note */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Servis Notu
            </label>
            <textarea
              value={formData.serviceNote || ''}
              onChange={(e) => setFormData({ ...formData, serviceNote: e.target.value })}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-red-500 focus:border-transparent"
              placeholder="Servis hakkında notlarınız..."
              rows={3}
              maxLength={1000}
            />
          </div>

          {/* Actions */}
          <div className="flex justify-end gap-3 pt-4 border-t border-gray-200">
            <button
              type="button"
              onClick={handleClose}
              disabled={mutation.isPending}
              className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 transition"
            >
              İptal
            </button>
            <button
              type="submit"
              disabled={mutation.isPending}
              className="px-4 py-2 text-sm font-medium text-white bg-red-600 rounded-lg hover:bg-red-700 disabled:opacity-50 transition"
            >
              {mutation.isPending ? 'Kaydediliyor...' : 'Kaydet'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ServiceEntryModal;