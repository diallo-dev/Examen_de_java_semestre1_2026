
package com.example.config;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;

public class ConfigLoader {
    
    
    public static Map<String, String> loadConfig(String configFilePath) {
        Map<String, String> config = new HashMap<>();
        Properties properties = new Properties();
        InputStream input = null;
        
        try {
            
            
            input = ConfigLoader.class.getClassLoader().getResourceAsStream(configFilePath);
            
            
            
            if (input == null) {
                System.out.println("⚠️ Fichier non trouvé dans resources/, " +
                                   "recherche dans le système de fichiers...");
                input = new FileInputStream(configFilePath);
            }
            
            
            
            properties.load(input);
            
            
            
            config.put("driver", properties.getProperty("driver"));
            config.put("url", properties.getProperty("url"));
            config.put("user", properties.getProperty("user"));
            config.put("password", properties.getProperty("password"));
            
            System.out.println(" Configuration chargée avec succès depuis : " + configFilePath);
            
        } catch (IOException e) {
            System.err.println(" Erreur lors du chargement de la configuration : " + e.getMessage());
            e.printStackTrace();
        } finally {
            if (input != null) {
                try {
                    input.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
        
        return config;
    }
}