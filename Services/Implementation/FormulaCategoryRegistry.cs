using System;
using System.Collections.Concurrent;
using Unit_Conversion_API.Models;
using Unit_Conversion_API.Repositories.Interfaces;

namespace Unit_Conversion_API.Services.Implementation
{
    // Centralized registry for categories that require formula-based conversions
    public class FormulaCategoryRegistry
    {
        // Categories that require formulas instead of factor-based conversion
        private readonly ConcurrentDictionary<string, bool> _formulaCategories = new(StringComparer.OrdinalIgnoreCase);

        // Key: "Category|From|To" -> ConversionFormula
        private readonly ConcurrentDictionary<string, ConversionFormula> _formulas = new(StringComparer.OrdinalIgnoreCase);

        // Constructor inspects optional repository categories and seeds known formula-based categories and formulas.
        public FormulaCategoryRegistry(IUnitRepository? unitRepository = null)
        {
            // Seed known categories (canonical keys)
            _formulaCategories["Temperature"] = true;
            _formulaCategories["PH"] = true;
            _formulaCategories["Decibel"] = true;
            _formulaCategories["SoundPressure"] = true;
            _formulaCategories["ElectricalGain"] = true;
            _formulaCategories["SignalStrength"] = true;
            _formulaCategories["EarthquakeMagnitude"] = true;
            _formulaCategories["FuelEconomy"] = true;
            _formulaCategories["Radioactivity"] = true;

            // If a unit repository is available, map any repository categories that look like
            // the known formula categories into the registry so lookups using repository category
            // names will succeed.
            if (unitRepository != null)
            {
                try
                {
                    var repoCats = unitRepository.GetUnitCategories();
                    foreach (var rc in repoCats)
                    {
                        if (string.IsNullOrWhiteSpace(rc)) continue;
                        var key = rc.Trim();
                        var lower = key.ToLowerInvariant();

                        if (lower.Contains("temp") || lower.Contains("temperature"))
                        {
                            _formulaCategories[key] = true;
                            // replicate temperature formulas under the repository category key so lookups succeed
                            CopyFormulasToCategory("Temperature", key);
                        }
                        else if (lower.Contains("ph") || lower.Contains("pH".ToLowerInvariant()))
                        {
                            _formulaCategories[key] = true;
                            CopyFormulasToCategory("PH", key);
                        }
                        else if (lower.Contains("decibel") || lower.Contains("db") || lower.Contains("dB".ToLowerInvariant()))
                        {
                            _formulaCategories[key] = true;
                            CopyFormulasToCategory("Decibel", key);
                        }
                        else if (lower.Contains("sound") || lower.Contains("pressure") || lower.Contains("spl"))
                        {
                            _formulaCategories[key] = true;
                            CopyFormulasToCategory("SoundPressure", key);
                        }
                        else if (lower.Contains("elect") || lower.Contains("gain"))
                        {
                            _formulaCategories[key] = true;
                            CopyFormulasToCategory("ElectricalGain", key);
                        }
                        else if (lower.Contains("signal") || lower.Contains("rssi") || lower.Contains("dbm"))
                        {
                            _formulaCategories[key] = true;
                            CopyFormulasToCategory("SignalStrength", key);
                        }
                        else if (lower.Contains("earth") || lower.Contains("quake") || lower.Contains("magnitude"))
                        {
                            _formulaCategories[key] = true;
                            CopyFormulasToCategory("EarthquakeMagnitude", key);
                        }
                        else if (lower.Contains("fuel") || lower.Contains("economy") || lower.Contains("mpg"))
                        {
                            _formulaCategories[key] = true;
                            CopyFormulasToCategory("FuelEconomy", key);
                        }
                        else if (lower.Contains("radio") || lower.Contains("becquerel") || lower.Contains("curie"))
                        {
                            _formulaCategories[key] = true;
                            CopyFormulasToCategory("Radioactivity", key);
                        }
                    }
                }
                catch
                {
                    // ignore repository inspection failures - registry still works with defaults
                }
            }

            // Seed temperature formulas
            AddFormula(new ConversionFormula { Category = "Temperature", FromUnit = "Celsius", ToUnit = "Fahrenheit", Formula = "(x * 9 / 5) + 32" });
            AddFormula(new ConversionFormula { Category = "Temperature", FromUnit = "Fahrenheit", ToUnit = "Celsius", Formula = "(x - 32) * 5 / 9" });
            AddFormula(new ConversionFormula { Category = "Temperature", FromUnit = "Celsius", ToUnit = "Kelvin", Formula = "x + 273.15" });
            AddFormula(new ConversionFormula { Category = "Temperature", FromUnit = "Kelvin", ToUnit = "Celsius", Formula = "x - 273.15" });
            AddFormula(new ConversionFormula { Category = "Temperature", FromUnit = "Fahrenheit", ToUnit = "Kelvin", Formula = "((x - 32) * 5 / 9) + 273.15" });
            AddFormula(new ConversionFormula { Category = "Temperature", FromUnit = "Kelvin", ToUnit = "Fahrenheit", Formula = "((x - 273.15) * 9 / 5) + 32" });

            // Seed pH formulas
            // Hydrogen ion concentration (mol/L) -> pH = -log10([H+])
            AddFormula(new ConversionFormula { Category = "PH", FromUnit = "HydrogenIonConcentration", ToUnit = "pH", Formula = "-log10(x)" });
            // pH -> Hydrogen ion concentration: [H+] = 10^(-pH)
            AddFormula(new ConversionFormula { Category = "PH", FromUnit = "pH", ToUnit = "HydrogenIonConcentration", Formula = "10^(-x)" });

            // Seed Decibel formulas
            // Power ratio -> dB: 10 * log10(P)
            AddFormula(new ConversionFormula { Category = "Decibel", FromUnit = "Ratio", ToUnit = "Decibel", Formula = "10 * log10(x)" });
            // dB -> power ratio: 10^(dB/10)
            AddFormula(new ConversionFormula { Category = "Decibel", FromUnit = "Decibel", ToUnit = "Ratio", Formula = "10^(x/10)" });
            // Amplitude (voltage/pressure) ratio -> dB: 20 * log10(A)
            AddFormula(new ConversionFormula { Category = "Decibel", FromUnit = "AmplitudeRatio", ToUnit = "Decibel", Formula = "20 * log10(x)" });
            // dB -> amplitude ratio: 10^(dB/20)
            AddFormula(new ConversionFormula { Category = "Decibel", FromUnit = "Decibel", ToUnit = "AmplitudeRatio", Formula = "10^(x/20)" });

            // Seed SoundPressure formulas (SPL)
            // Pascal -> dB SPL: 20 * log10(p / p0), p0 = 20 µPa = 0.00002 Pa
            AddFormula(new ConversionFormula { Category = "SoundPressure", FromUnit = "Pascal", ToUnit = "Decibel", Formula = "20 * log10(x / 0.00002)" });
            // dB SPL -> Pascal: 10^(dB/20) * p0
            AddFormula(new ConversionFormula { Category = "SoundPressure", FromUnit = "Decibel", ToUnit = "Pascal", Formula = "10^(x/20) * 0.00002" });

            // Electrical gain examples
            // Voltage gain (ratio) -> dB: 20 * log10(Vout/Vin)
            AddFormula(new ConversionFormula { Category = "ElectricalGain", FromUnit = "VoltageRatio", ToUnit = "Decibel", Formula = "20 * log10(x)" });
            AddFormula(new ConversionFormula { Category = "ElectricalGain", FromUnit = "Decibel", ToUnit = "VoltageRatio", Formula = "10^(x/20)" });

            // Signal strength examples (RSSI / dBm conversions relative to mW)
            // mW -> dBm: 10 * log10(P [mW])
            AddFormula(new ConversionFormula { Category = "SignalStrength", FromUnit = "MilliWatt", ToUnit = "dBm", Formula = "10 * log10(x)" });
            // dBm -> mW: 10^(dBm/10)
            AddFormula(new ConversionFormula { Category = "SignalStrength", FromUnit = "dBm", ToUnit = "MilliWatt", Formula = "10^(x/10)" });

            // Earthquake magnitude mapping (approx): Richter magnitude relates to log10 of energy — examples
            // Relative energy ratio -> Richter (approx): 1.5 * log10(EnergyRatio)
            AddFormula(new ConversionFormula { Category = "EarthquakeMagnitude", FromUnit = "EnergyRatio", ToUnit = "Richter", Formula = "1.5 * log10(x)" });
            // Richter -> Energy ratio: 10^(Magnitude / 1.5)
            AddFormula(new ConversionFormula { Category = "EarthquakeMagnitude", FromUnit = "Richter", ToUnit = "EnergyRatio", Formula = "10^(x / 1.5)" });

            // Fuel economy examples
            // L/100km -> mpg (US) conversion: mpg = 235.214583 / (L/100km)
            AddFormula(new ConversionFormula { Category = "FuelEconomy", FromUnit = "LPer100km", ToUnit = "MPG_US", Formula = "235.214583 / x" });
            // mpg -> L/100km
            AddFormula(new ConversionFormula { Category = "FuelEconomy", FromUnit = "MPG_US", ToUnit = "LPer100km", Formula = "235.214583 / x" });

            // Radioactivity examples
            // Becquerel -> Curie: 1 Ci = 3.7e10 Bq
            AddFormula(new ConversionFormula { Category = "Radioactivity", FromUnit = "Becquerel", ToUnit = "Curie", Formula = "x / 3.7e10" });
            AddFormula(new ConversionFormula { Category = "Radioactivity", FromUnit = "Curie", ToUnit = "Becquerel", Formula = "x * 3.7e10" });
        }

        private string BuildKey(string category, string from, string to)
        {
            return $"{category}|{from}|{to}";
        }

        private void CopyFormulasToCategory(string sourceCategory, string targetCategory)
        {
            try
            {
                foreach (var f in _formulas.Values)
                {
                    if (string.Equals(f.Category, sourceCategory, StringComparison.OrdinalIgnoreCase))
                    {
                        var copy = new ConversionFormula
                        {
                            Category = targetCategory,
                            FromUnit = f.FromUnit,
                            ToUnit = f.ToUnit,
                            Formula = f.Formula
                        };
                        // ignore return value; if already present, that's fine
                        _formulas.TryAdd(BuildKey(copy.Category, copy.FromUnit, copy.ToUnit), copy);
                    }
                }
            }
            catch
            {
                // swallow errors; copying is best-effort
            }
        }

        public bool RequiresFormula(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return false;
            return _formulaCategories.ContainsKey(category);
        }

        public bool TryGetFormula(string category, string fromUnit, string toUnit, out string formula)
        {
            formula = string.Empty;
            var key = BuildKey(category, fromUnit, toUnit);
            if (_formulas.TryGetValue(key, out var cf))
            {
                formula = cf.Formula;
                return true;
            }

            return false;
        }

        public bool AddFormula(ConversionFormula formula)
        {
            if (formula == null) return false;
            var key = BuildKey(formula.Category, formula.FromUnit, formula.ToUnit);
            return _formulas.TryAdd(key, formula);
        }

        public bool AddFormulaCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return false;
            _formulaCategories[category] = true;
            return true;
        }
    }
}
